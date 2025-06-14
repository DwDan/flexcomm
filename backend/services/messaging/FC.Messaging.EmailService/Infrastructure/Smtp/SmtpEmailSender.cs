using System.Net;
using System.Net.Mail;
using FC.Messaging.EmailService.Domain;
using Microsoft.Extensions.Options;

namespace FC.Messaging.EmailService.Infrastructure.Smtp
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _client;
        private readonly string _remetente;

        public SmtpEmailSender(IOptions<SmtpSettings> options)
        {
            var settings = options.Value;

            _remetente = settings.From;

            _client = new SmtpClient(settings.Host, settings.Port)
            {
                Credentials = new NetworkCredential(settings.Username, settings.Password),
                EnableSsl = settings.EnableSsl
            };
        }

        public async Task EnviarAsync(string destinatario, string assunto, string corpoHtml, CancellationToken cancellationToken = default)
        {
            var mensagem = new MailMessage(_remetente, destinatario, assunto, corpoHtml)
            {
                IsBodyHtml = true
            };

            await _client.SendMailAsync(mensagem, cancellationToken);
        }
    }
}
