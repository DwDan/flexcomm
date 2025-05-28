using FC.Auth.Domain.Enums;
using MediatR;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommand : IRequest<bool>
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; }
    }
}
