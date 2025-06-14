using FC.Auth.Domain.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class UsuarioAlteradoEventHandler : INotificationHandler<UsuarioAlteradoEvent>
    {
        private readonly ILogger<UsuarioAlteradoEventHandler> _logger;

        public UsuarioAlteradoEventHandler(ILogger<UsuarioAlteradoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UsuarioAlteradoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Usuário alterado: Id = {Id}, Nome = {Nome}, Email = {Email}",
                notification.Id, notification.Nome, notification.Email);

            return Task.CompletedTask;
        }
    }
}
