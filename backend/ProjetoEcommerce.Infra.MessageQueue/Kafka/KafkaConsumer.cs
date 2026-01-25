using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.MessageQueue.Kafka
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync<T>(string topic, Func<T, Task> callback);
    }

    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IConfiguration _configuration;

        public KafkaConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConsumeAsync<T>(string topic, Func<T, Task> callback)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["MessageQueue:Kafka:BootstrapServers"],
                GroupId = "ecommerce-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(topic);

                while (true)
                {
                    try
                    {
                        var result = consumer.Consume();
                        var message = JsonSerializer.Deserialize<T>(result.Message.Value);
                        await callback(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem Kafka: {ex.Message}");
                    }
                }
            }
        }
    }
}
