using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public class RabbitMQRetriever<T> : IRabbitMQRetriever<T>
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private TaskCompletionSource<T> _responseCompletionSource;

        public RabbitMQRetriever(IRabbitMQConnection rabbitMQConnection) 
        {
            _rabbitMQConnection= rabbitMQConnection;
        }

        public Task<T> PublishMessageAndGetResponse(object message)
        {
            _responseCompletionSource = new TaskCompletionSource<T>();
            
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var correlationId = Guid.NewGuid().ToString();
            using var channel = _rabbitMQConnection.CreateModel();
            channel.ExchangeDeclare("exchange.movies", ExchangeType.Topic, durable: true);
            channel.QueueBind("queue.movie.actors", "exchange.actors", "movie.actors");

            channel.BasicPublish(exchange: "exchange.movies",
                                  routingKey: "movie.actors.ids",
                                  body: body,
                                  mandatory: true);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var responseMessage = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<T>(responseMessage);
                _responseCompletionSource.SetResult(message);

                Console.WriteLine("Received response message: " + responseMessage);
            };


            channel.BasicConsume(consumer: consumer,
                                  queue: "queue.movie.actors",
                                  autoAck: true);

            return _responseCompletionSource.Task;
        }
    }
}
