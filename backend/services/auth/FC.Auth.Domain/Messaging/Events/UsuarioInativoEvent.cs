using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Messaging.Events
{
    public class UsuarioInativoEvent : IDomainEvent
    {
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public UsuarioInativoEvent(Guid usuarioId, string nome, string email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }
    }
}