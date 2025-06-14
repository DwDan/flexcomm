using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Messaging.Events
{
    public class EmailUsuarioConfirmadoEvent : IDomainEvent
    {
        public string Nome { get; }
        public string Email { get; }

        public EmailUsuarioConfirmadoEvent(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}