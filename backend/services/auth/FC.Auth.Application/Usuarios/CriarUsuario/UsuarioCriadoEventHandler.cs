using FC.Auth.Domain.Messaging.Events;
using FC.BuildingBlocks.Application;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using FC.BuildingBlocks.Domain.Messaging.Events;
using FC.BuildingBlocks.Domain.Security;
using MediatR;
using Microsoft.Extensions.Options;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class UsuarioCriadoEventHandler : INotificationHandler<UsuarioCriadoEvent>
    {
        private readonly IEventBusProducer _eventProducer;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IOptions<AplicacaoSettings> _options;

        public UsuarioCriadoEventHandler(IEventBusProducer eventProducer, IJwtTokenGenerator jwtTokenGenerator, IOptions<AplicacaoSettings> options)
        {
            _eventProducer = eventProducer;
            _jwtTokenGenerator = jwtTokenGenerator;
            _options = options;
        }

        public async Task Handle(UsuarioCriadoEvent notification, CancellationToken cancellationToken)
        {
            var settings = _options.Value;
            var token = _jwtTokenGenerator.GenerateTokenEmailConfirmation(notification);
            var urlConfirmacao = $"{settings.UrlBase}/api/usuario/confirmar-email?token={token}";

            var emailEvent = new EnviarEmailEvent
            {
                To = notification.Email,
                Subject = "Bem-vindo!",
                HtmlBody = $@"
                <p>Cadastro realizado com sucesso.</p>
                <p>Para confirmar seu e-mail, clique no link abaixo:</p>
                <p><a href=""{urlConfirmacao}"">Confirmar e-mail</a></p>
            "
            };

            await _eventProducer.PublicarAsync("email-topic", emailEvent);
        }
    }
}
