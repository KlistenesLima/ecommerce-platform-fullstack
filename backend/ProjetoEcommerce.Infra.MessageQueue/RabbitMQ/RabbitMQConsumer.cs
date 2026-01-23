using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.MessageQueue.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        Task ConsumeAsync<T>(string queue, Func<T?, Task> handler, CancellationToken cancellationToken = default);
    }

    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IConfiguration _configuration;

        public RabbitMQConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConsumeAsync<T>(string queue, Func<T?, Task> handler, CancellationToken cancellationToken = default)
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

            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                try
                {
                    await handler(message);
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch
                {
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            channel.BasicConsume(
                queue: queue,
                autoAck: false,
                consumer: consumer);

            // Keep running until cancellation
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}