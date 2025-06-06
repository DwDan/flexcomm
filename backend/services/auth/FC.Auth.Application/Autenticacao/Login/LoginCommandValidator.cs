using FluentValidation;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithErrorCode(Login_EmailObrigatorio);

            RuleFor(command => command.Senha)
                .NotEmpty().WithErrorCode(Login_SenhaObrigatoria);
        }

        public static string Login_EmailObrigatorio => "Login.EmailObrigatorio";
        public static string Login_SenhaObrigatoria => "Login.SenhaObrigatoria";
    }
}
