using FC.Auth.Application.Usuarios.AlterarUsuario.DTO;
using MediatR;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public AlterarUsuarioNomeCompletoDto NomeCompleto { get; set; }
        public AlterarUsuarioEnderecoDto? Endereco { get; set; }
        public AlterarUsuarioNumeroTelefoneDto? Telefone { get; set; }
    }
}
