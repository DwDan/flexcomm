using FC.Auth.Domain.Messaging.Events;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using MediatR;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class EmailUsuarioConfirmadoEventHandler : INotificationHandler<EmailUsuarioConfirmadoEvent>
    {
        private readonly IEventBusProducer _eventProducer;

        public EmailUsuarioConfirmadoEventHandler(IEventBusProducer eventProducer)
        {
            _eventProducer = eventProducer;
        }

        public async Task Handle(EmailUsuarioConfirmadoEvent notification, CancellationToken cancellationToken)
        {
            var emailEvent = new EnviarEmailEvent
            {
                To = notification.Email,
                Subject = "Confirmação de E-mail",
                HtmlBody = $@"
                <p>Olá, {notification.Nome}!</p>
                <p>Seu e-mail foi confirmado com sucesso.</p>
            "
            };

            await _eventProducer.PublicarAsync("email-topic", emailEvent);
        }
    }
}