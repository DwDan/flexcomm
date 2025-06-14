using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Messaging.Events
{
    public class UsuarioAlteradoEvent : IDomainEvent
    {
        public Guid Id { get; }
        public string Nome { get; }
        public string Email { get; }

        public UsuarioAlteradoEvent(Guid usuarioId, string nome, string email)
        {
            Id = usuarioId;
            Nome = nome;
            Email = email;
        }
    }
}
