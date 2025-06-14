using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;

namespace FC.Auth.Domain.Messaging.Events
{
    public class UsuarioCriadoEvent : IDomainEvent, IUsuario
    {
        public Guid Id { get; }
        public string Email { get; }
        public bool Ativo { get; }
        public bool EmailConfirmado { get; }

        public UsuarioCriadoEvent(Guid usuarioId, string email)
        {
            Id = usuarioId;
            Email = email;
            Ativo = true;
        }
    }
}
