using MediatR;

namespace FC.Auth.Application.Usuarios.AtivarUsuario
{
    public class AtivarUsuarioCommand : IRequest<bool>  
    {
        public Guid Id { get; set; }

        public AtivarUsuarioCommand(Guid id)
        {
            Id = id;
        }
    }
}
