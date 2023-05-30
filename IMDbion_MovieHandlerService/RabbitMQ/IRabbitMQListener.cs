namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public interface IRabbitMQListener
    {
        public Task Subscribe<T>(string queueName, string exchangeName, string routingKey, Action<T> handler);
    }
}
