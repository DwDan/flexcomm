using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.Messaging.EmailService.Domain;

namespace FC.Messaging.EmailService.Application
{
    public class EnviarEmailHandler : IIntegrationEventHandler<EnviarEmailEvent>
    {
        private readonly IEmailSender _emailSender;

        public EnviarEmailHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task HandleAsync(EnviarEmailEvent evento, CancellationToken cancellationToken)
        {
            await _emailSender.EnviarAsync(evento.To, evento.Subject, evento.HtmlBody, cancellationToken);
        }
    }
}
