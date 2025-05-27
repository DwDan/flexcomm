using FC.Auth.Domain.Entities;
using FluentValidation;

namespace FC.Auth.Domain.Validation
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public static string NomeObrigatorio => "O nome é obrigatório.";
        public static string EmailObrigatorio => "O e-mail é obrigatório.";
        public static string SenhaObrigatoria => "A senha é obrigatória.";

        public static string NomeInvalido => "O nome deve ter no mínimo 3 caracteres.";
        public static string EmailInvalido => "O e-mail é inválido.";
        public static string SenhaInvalida => "A senha deve ter no mínimo 6 caracteres.";

        public UsuarioValidation()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage(NomeObrigatorio)
                .MinimumLength(3).WithMessage(NomeInvalido);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(EmailObrigatorio)
                .EmailAddress().WithMessage(EmailInvalido);

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage(SenhaObrigatoria)
                .MinimumLength(6).WithMessage(SenhaInvalida);
        }
    }
}
