using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.MessageQueue.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(string topic, string key, T message);
    }

    public class KafkaProducer : IKafkaProducer
    {
        private readonly IConfiguration _configuration;

        public KafkaProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ProduceAsync<T>(string topic, string key, T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["MessageQueue:Kafka:BootstrapServers"]
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var json = JsonSerializer.Serialize(message);
                var result = await producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = json
                });

                Console.WriteLine($"Mensagem enviada para Kafka: {result.Topic}");
            }
        }
    }
}
