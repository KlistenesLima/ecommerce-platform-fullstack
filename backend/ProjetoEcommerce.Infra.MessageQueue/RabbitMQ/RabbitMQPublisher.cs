using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.MessageQueue.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        Task PublishAsync<T>(string exchange, string routingKey, T message);
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConfiguration _configuration;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task PublishAsync<T>(string exchange, string routingKey, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["MessageQueue:RabbitMQ:HostName"] ?? "localhost",
                Port = int.Parse(_configuration["MessageQueue:RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["MessageQueue:RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["MessageQueue:RabbitMQ:Password"] ?? "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Se exchange for vazio, usa Default Exchange (envio direto p/ fila)
            if (!string.IsNullOrEmpty(exchange))
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
            }

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: exchange ?? "",
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        }
    }
}
