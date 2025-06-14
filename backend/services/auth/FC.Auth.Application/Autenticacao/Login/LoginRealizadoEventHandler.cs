using FC.Auth.Domain.Messaging.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginRealizadoEventHandler : INotificationHandler<LoginRealizadoEvent>
    {
        private readonly ILogger<LoginRealizadoEventHandler> _logger;

        public LoginRealizadoEventHandler(ILogger<LoginRealizadoEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(LoginRealizadoEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login realizado: UsuarioId = {UsuarioId}, Email = {Email}, DataHora = {DataHora}",
                notification.UsuarioId, notification.Email, notification.DataHora);

            return Task.CompletedTask;
        }
    }
}
