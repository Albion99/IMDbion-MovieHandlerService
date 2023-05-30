namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public interface IRabbitMQPublish
    {
        public Task Publish<T>(T message, string exchangeName, string routingKey);
    }
}
