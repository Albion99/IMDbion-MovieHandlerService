using IMDbion_MovieHandlerService.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public class RabbitMQRetriever<T> : IRabbitMQRetriever<T>
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private static readonly Dictionary<string, TaskCompletionSource<T>> _requests = new();
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(30);

        private readonly string exchange = "actors";
        private readonly string routingKey = "queue.movie";
        private readonly string listenQueue = "queue.actors";

        public RabbitMQRetriever(IRabbitMQConnection rabbitMQConnection) 
        {
            _rabbitMQConnection = rabbitMQConnection;

            using var channel = _rabbitMQConnection.CreateModel();

            channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
            channel.QueueDeclare(listenQueue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(listenQueue, exchange, listenQueue);
        }

        public async Task<T> PublishMessageAndGetResponse(object message)
        {
            var correlationId = Guid.NewGuid().ToString();

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            using var channel = _rabbitMQConnection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = listenQueue;

            var requestCompletionSource = new TaskCompletionSource<T>();
            _requests.TryAdd(correlationId, requestCompletionSource);

            channel.BasicPublish(exchange: exchange,
                                  routingKey: routingKey,
                                  basicProperties: properties,
                                  body: body,
                                  mandatory: true);

            Debug.WriteLine($" [x] send '{message}' from movie service");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var correlationId = eventArgs.BasicProperties.CorrelationId;

                var body = eventArgs.Body.ToArray();
                var responseMessage = Encoding.UTF8.GetString(body);

                var actors = JsonConvert.DeserializeObject<T>(responseMessage);

                if (_requests.Remove(correlationId, out var requestCompletionSource))
                {
                    requestCompletionSource.TrySetResult(actors);
                }

                Debug.WriteLine($" [x] Received '{actors}' in movie service");
            };

            channel.BasicConsume(listenQueue, true, consumer);

            var timeoutTask = Task.Delay(RequestTimeout);
            var completedTask = await Task.WhenAny(requestCompletionSource.Task, timeoutTask);

            if (completedTask == timeoutTask)
            {
                _requests.Remove(correlationId, out _);
                throw new TimeoutException("Request timed out.");
            }

            return await requestCompletionSource.Task;
        }
    }
}

