using RabbitMQ.Client;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        public IModel CreateModel();
        public void Close();
    }
}
