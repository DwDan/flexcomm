using FC.Auth.Domain.Enums;
using FC.Auth.Domain.Validation;
using FC.BuildingBlocks.Domain;
using FluentValidation.Results;

namespace FC.Auth.Domain.Entities
{
    public class Usuario : Entity, IAggregateRoot
    {
        public string? Nome { get; private set; }
        public string? Email { get; private set; }
        public string? SenhaHash { get; private set; }
        public bool Ativo { get; private set; }
        public PerfilUsuario Perfil { get; private set; }

        protected Usuario() { }

        public Usuario(string nome, string email, string senhaHash, PerfilUsuario perfil)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            Ativo = true;
            Perfil = perfil;
        }

        public ValidationResult Validar() 
        {
            return new UsuarioValidation().Validate(this);
        }
    }
}
