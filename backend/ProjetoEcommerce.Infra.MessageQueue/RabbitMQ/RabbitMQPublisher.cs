using ProjetoEcommerce.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProjetoEcommerce.Infra.MessageQueue.RabbitMQ
{
    // Agora implementa a interface específica
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            try 
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch
            {
                _connection = null;
                _channel = null;
            }
        }

        public void Publish<T>(T message, string queueName)
        {
            if (_channel == null) return;

            _channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
