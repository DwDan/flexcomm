using FluentValidation;

namespace FC.Auth.Application.Autenticacao.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage(LoginErrors.EmailObrigatorio);

            RuleFor(command => command.Senha)
                .NotEmpty().WithMessage(LoginErrors.SenhaObrigatoria);
        }
    }
}
