using FC.BuildingBlocks.Domain.Security;

namespace FC.BuildingBlocks.Integration.Tests.DTO
{
    public class UsuarioDto : IUsuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public bool EmailConfirmado { get; set; }
    }
}
