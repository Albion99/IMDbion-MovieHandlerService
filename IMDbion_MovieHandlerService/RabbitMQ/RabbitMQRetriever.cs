using IMDbion_MovieHandlerService.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public class RabbitMQRetriever<T> : IRabbitMQRetriever<T>
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private TaskCompletionSource<T> _responseCompletionSource;

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
            _responseCompletionSource = new TaskCompletionSource<T>();
            
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var correlationId = Guid.NewGuid().ToString();

            using var channel = _rabbitMQConnection.CreateModel();

            channel.BasicPublish(exchange: exchange,
                                  routingKey: routingKey,
                                  body: body,
                                  mandatory: true);

            Debug.WriteLine($" [x] send '{message}' from movie service");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var responseMessage = Encoding.UTF8.GetString(body);

                var actors = JsonConvert.DeserializeObject<T>(responseMessage);
                _responseCompletionSource.TrySetResult(actors);

                Debug.WriteLine($" [x] Received '{actors}' in movie service");
            };

            channel.BasicConsume(listenQueue, true, consumer);

            return await _responseCompletionSource.Task;
        }
    }
}

