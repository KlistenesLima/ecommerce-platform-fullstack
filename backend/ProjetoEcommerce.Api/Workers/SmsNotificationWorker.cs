using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEcommerce.Domain.Events;
using ProjetoEcommerce.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Workers
{
    public class SmsNotificationWorker : BackgroundService
    {
        private readonly ILogger<SmsNotificationWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "order-created";

        public SmsNotificationWorker(ILogger<SmsNotificationWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                
                _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            catch (Exception ex) { _logger.LogError($"RabbitMQ Init Error: {ex.Message}"); }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel == null) return Task.CompletedTask;

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                try 
                {
                    var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
                    if (orderEvent != null)
                    {
                        await ProcessNotification(orderEvent);
                    }
                }
                catch (Exception ex) { _logger.LogError($"Erro processando mensagem: {ex.Message}"); }
            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessNotification(OrderCreatedEvent order)
        {
            // LOG NOVO PARA IDENTIFICAR QUE É O CÓDIGO NOVO
            _logger.LogWarning($"[WORKER SMTP] Tentando enviar email REAL para {order.Email}...");

            try 
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    
                    var subject = $"Pedido Confirmado! #{order.OrderId.ToString().Substring(0, 8)}";
                    
                    var body = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; background-color: #f9f9f9;'>
                            <h2 style='color: #27ae60;'>Pagamento Aprovado!</h2>
                            <p>Olá, <strong>{order.CustomerName}</strong>.</p>
                            <p>Seu pedido foi processado com sucesso.</p>
                            <hr>
                            <h3>Detalhes:</h3>
                            <p><b>Total:</b> R$ {order.TotalAmount:F2}</p>
                            <p><b>Data:</b> {order.CreatedAt}</p>
                            <br>
                            <p>Obrigado por comprar conosco!</p>
                        </div>";

                    await emailService.SendEmailAsync(order.Email, subject, body);
                    _logger.LogInformation($"[WORKER SMTP] Email enviado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[WORKER ERRO] Falha ao enviar email: {ex.Message}");
            }
        }
    }
}
