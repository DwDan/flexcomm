using MediatR;

namespace FC.Auth.Application.Usuarios.InativarUsuario
{
    public class InativarUsuarioCommand : IRequest<bool>    
    {
        public Guid Id { get; set; }

        public InativarUsuarioCommand(Guid id)
        {
            Id = id;
        }
    }
}
