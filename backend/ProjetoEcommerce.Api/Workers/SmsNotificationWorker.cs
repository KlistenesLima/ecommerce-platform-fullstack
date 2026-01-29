using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjetoEcommerce.Infra.MessageQueue.RabbitMQ;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Workers
{
    public class SmsNotificationWorker : BackgroundService
    {
        private readonly IRabbitMQConsumer _consumer;
        private readonly ILogger<SmsNotificationWorker> _logger;
        private readonly IConfiguration _configuration;

        public SmsNotificationWorker(IRabbitMQConsumer consumer, ILogger<SmsNotificationWorker> logger, IConfiguration configuration)
        {
            _consumer = consumer;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(">>> Worker de Notificações Ativo (Email Real + SMS Mock)");

            await _consumer.ConsumeAsync<NotificationDTO>("sms_notifications_queue", async (message) =>
            {
                if (message != null)
                {
                    // Envia em paralelo
                    var t1 = SendMockSms(message);
                    var t2 = SendRealEmail(message);
                    
                    await Task.WhenAll(t1, t2);
                }
            }, stoppingToken);
        }

        private Task SendMockSms(NotificationDTO data)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SMS MOCK] Enviado para {data.Phone}: {data.Message}");
            Console.ForegroundColor = originalColor;
            return Task.CompletedTask;
        }

        private async Task SendRealEmail(NotificationDTO data)
        {
            // Pega config do appsettings ou usa valores padrao que vão falhar se nao configurar
            var smtpHost = _configuration["Email:Host"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:Port"] ?? "587");
            var smtpUser = _configuration["Email:User"]; // SEU GMAIL
            var smtpPass = _configuration["Email:Password"]; // SUA SENHA DE APP DO GMAIL

            if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[EMAIL ERROR] Credenciais de E-mail não configuradas no appsettings.json!");
                Console.ResetColor();
                // Lança exceção para testar a DLQ (Dead Letter Queue)
                throw new Exception("Credenciais de email inválidas. Mensagem enviada para DLQ."); 
            }

            try 
            {
                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUser, "LuxeStore"),
                    Subject = $"Pedido Confirmado: {data.OrderId}",
                    Body = $"<h1>Olá, {data.CustomerName}!</h1><p>{data.Message}</p><p>Obrigado por comprar conosco.</p>",
                    IsBodyHtml = true,
                };
                
                // Se o email do usuario for invalido/mock, enviamos para o próprio remetente para teste
                var targetEmail = data.Email.Contains("@") ? data.Email : smtpUser;
                mailMessage.To.Add(targetEmail);

                Console.WriteLine($"[EMAIL REAL] Conectando ao SMTP {smtpHost}...");
                await client.SendMailAsync(mailMessage);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[EMAIL REAL] Sucesso! Enviado para {targetEmail}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[EMAIL FALHOU] {ex.Message}");
                Console.ResetColor();
                throw; // Joga o erro pra cima pro RabbitMQ mandar pra DLQ
            }
        }
    }

    public class NotificationDTO
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
