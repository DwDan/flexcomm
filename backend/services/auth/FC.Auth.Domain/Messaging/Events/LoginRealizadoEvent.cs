using MediatR;

namespace FC.Auth.Domain.Messaging.Events
{
    public class LoginRealizadoEvent : INotification
    {
        public Guid UsuarioId { get; }
        public string Email { get; }
        public DateTime DataHora { get; }

        public LoginRealizadoEvent(Guid usuarioId, string email)
        {
            UsuarioId = usuarioId;
            Email = email;
            DataHora = DateTime.UtcNow;
        }
    }
}
