using MediatR;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginCommand : IRequest<LoginCommandResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}
