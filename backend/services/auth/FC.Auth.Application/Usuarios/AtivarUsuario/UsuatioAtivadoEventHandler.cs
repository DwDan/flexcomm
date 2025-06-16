using FC.Auth.Domain.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Auth.Application.Usuarios.InativarUsuario
{
    public class UsuatioAtivadoEventHandler : INotificationHandler<UsuarioAtivadoEvent>
    {
        private ILogger<UsuatioAtivadoEventHandler> _logger;

        public UsuatioAtivadoEventHandler(ILogger<UsuatioAtivadoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UsuarioAtivadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Usuário ativado: {Id}, {Email}", notification.UsuarioId, notification.Email);

            return Task.CompletedTask;
        }
    }
}