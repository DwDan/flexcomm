using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Messaging.Events
{
    public class UsuarioAtivoEvent : IDomainEvent
    {
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public UsuarioAtivoEvent(Guid usuarioId, string nome, string email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }
    }
}