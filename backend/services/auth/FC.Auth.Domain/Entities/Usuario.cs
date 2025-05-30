using FC.Auth.Domain.Validation;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;
using FluentValidation.Results;

namespace FC.Auth.Domain.Entities
{
    public class Usuario : Entity, IUsuario, IAggregateRoot
    {
        public string? Nome { get; private set; }
        public string? Email { get; private set; }
        public string? SenhaHash { get; private set; }
        public bool Ativo { get; private set; }

        protected Usuario() { }

        public Usuario(string nome, string email)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Ativo = true;
        }

        public ValidationResult Validar() 
        {
            return new UsuarioValidation().Validate(this);
        }

        public void DefinirSenhaCriptografada(string senhaCriptografada)
        {
            SenhaHash = senhaCriptografada;
        }
    }
}
