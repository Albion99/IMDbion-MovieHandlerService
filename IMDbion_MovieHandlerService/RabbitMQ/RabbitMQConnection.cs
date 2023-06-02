using RabbitMQ.Client;

namespace IMDbion_MovieHandlerService.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQConnection(string hostname, string username, string password)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
        }

        public IModel CreateModel()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                var factory = Uri.IsWellFormedUriString(_hostname, UriKind.Absolute)
                    ? new ConnectionFactory
                    {
                        Uri = new Uri(_hostname),
                        UserName = _username,
                        Password = _password
                    }
                    : new ConnectionFactory
                    {
                        HostName = _hostname,
                        UserName = _username,
                        Password = _password
                    };
                _connection = factory.CreateConnection();
            }

            return _connection.CreateModel();
        }

        public void Close()
        {
            _connection?.Close();
        }
    }
}
