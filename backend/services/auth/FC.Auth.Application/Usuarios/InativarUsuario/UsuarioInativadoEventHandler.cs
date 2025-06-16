using FC.Auth.Domain.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Auth.Application.Usuarios.InativarUsuario
{
    public class UsuarioInativadoEventHandler : INotificationHandler<UsuarioInativadoEvent>
    {
        private ILogger<UsuarioInativadoEventHandler> _logger;

        public UsuarioInativadoEventHandler(ILogger<UsuarioInativadoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UsuarioInativadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Usuário inativado: {Id}, {Email}", notification.UsuarioId, notification.Email);

            return Task.CompletedTask;
        }
    }
}
