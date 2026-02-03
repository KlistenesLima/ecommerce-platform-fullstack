using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjetoEcommerce.Domain.Interfaces; // Agora vai encontrar
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.Cloud.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var host = _configuration["Email:Host"];
                var port = int.Parse(_configuration["Email:Port"] ?? "587");
                var user = _configuration["Email:User"];
                var pass = _configuration["Email:Password"];

                if (string.IsNullOrEmpty(user)) 
                {
                    _logger.LogWarning("[EMAIL] Credenciais vazias.");
                    return;
                }

                // Correção do 'From': Garante que tenha @gmail.com se o user vier sem
                var fromEmail = user.Contains("@") ? user : $"{user}@gmail.com";

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "Ecommerce System"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"[EMAIL REAL] Enviado para {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERRO EMAIL] {ex.Message}");
            }
        }
    }
}
