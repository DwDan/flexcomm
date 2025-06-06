using FluentValidation;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithErrorCode("Login.EmailObrigatorio");

            RuleFor(command => command.Senha)
                .NotEmpty().WithErrorCode("Login.SenhaObrigatoria");
        }
    }
}
