using System.Text;
using System.Threading.Channels;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public interface IRabbitMQRetriever<T>
    {
        public Task<T> PublishMessageAndGetResponse(object message);
    }
}
