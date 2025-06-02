using MediatR;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public AlterarUsuarioNomeCompleto NomeCompleto { get; set; }
        public AlterarUsuarioEndereco? Endereco { get; set; }
        public AlterarUsuarioNumeroTelefone? Telefone { get; set; }
    }
}
