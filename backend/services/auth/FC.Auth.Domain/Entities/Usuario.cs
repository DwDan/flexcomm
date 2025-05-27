using FC.Auth.Domain.Enums;
using FC.Auth.Domain.Validation;
using FC.BuildingBlocks.Domain;
using FluentValidation.Results;

namespace FC.Auth.Domain.Entities
{
    public class Usuario : Entity
    {
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? PasswordHash { get; private set; }
        public bool Active { get; private set; }
        public UserRole Role { get; private set; }

        protected Usuario() { }

        public Usuario(string name, string email, string passwordHash, UserRole role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Active = true;
            Role = role;
        }

        public ValidationResult Validar() 
        {
            return new UsuarioValidation().Validate(this);
        }
    }
}
