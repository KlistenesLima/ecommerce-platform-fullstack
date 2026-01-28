using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
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
                Password = _configuration["MessageQueue:RabbitMQ:Password"] ?? "guest",
                DispatchConsumersAsync = true // Permite handlers assíncronos reais
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // 1. QoS (PREFETCH): Garante que a API pegue apenas 1 mensagem por vez
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            // 2. DLQ (DEAD LETTER QUEUE): Onde as mensagens falhas vão parar
            var dlqName = $"{queue}_dlq";
            channel.QueueDeclare(queue: dlqName, durable: true, exclusive: false, autoDelete: false);

            // 3. FILA PRINCIPAL (Com configuração para jogar erros na DLQ)
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" }, // Exchange padrão
                { "x-dead-letter-routing-key", dlqName } // Roteia para a DLQ
            };

            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args); // <--- Argumentos vitais

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(json);

                    // Executa a lógica (Envio de Email/SMS)
                    await handler(message);

                    // Se deu tudo certo, remove a mensagem da fila
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RABBITMQ ERROR] Processamento falhou: {ex.Message}");
                    
                    // NACK: Rejeita a mensagem. Como 'requeue' é false, ela vai para a DLQ automaticamente
                    channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);

            // Mantém a conexão viva
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
