//using IMDbion_MovieHandlerService.Models;

//namespace IMDbion_MovieHandlerService.RabbitMQ
//{
//    public class RabbitMQBackgroundService : BackgroundService
//    {
//        private readonly IRabbitMQListener _rabbitMQListener;
//        private readonly string _queueName;
//        private readonly string _exchangeName;
//        private readonly string _routingKey;

//        public RabbitMQBackgroundService(IRabbitMQListener rabbitMQListener, string queueName, string exchangeName, string routingKey)
//        {
//            _rabbitMQListener = rabbitMQListener;
//            _queueName = queueName;
//            _exchangeName = exchangeName;
//            _routingKey = routingKey;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            await _rabbitMQListener.Subscribe<Movie>(_queueName, _exchangeName, _routingKey, HandleMessage<Movie>);

//            await Task.Delay(-1, stoppingToken);
//        }

//        private void HandleMessage<T>(T message)
//        {
//            // Handle the received message here
//        }
//    }
//}
