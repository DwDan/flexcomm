using MediatR;

namespace FC.Auth.Application.Usuarios.ConfirmarEmail
{
    public class ConfirmarEmailUsuarioCommand :  IRequest<bool>
    {
        public ConfirmarEmailUsuarioCommand(string token)
        {
            Token = token;
        }

        public string Token { get; set; } = string.Empty;
    }
}
