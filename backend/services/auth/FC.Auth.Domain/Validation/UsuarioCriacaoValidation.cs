using FC.Auth.Domain.Entities;
using FluentValidation;

namespace FC.Auth.Domain.Validation
{
    public class UsuarioCriacaoValidation : AbstractValidator<Usuario>
    {
        public static string NomeObrigatorio => "O nome é obrigatório.";
        public static string EmailObrigatorio => "O e-mail é obrigatório.";
        public static string SenhaObrigatoria => "A senha é obrigatória.";

        public static string NomeInvalido => "O nome deve ter no mínimo 3 caracteres.";
        public static string EmailInvalido => "O e-mail é inválido.";
        public static string SenhaInvalida => "A senha precisa estar criptografada.";

        public UsuarioCriacaoValidation()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithMessage(NomeObrigatorio)
                .MinimumLength(3).WithMessage(NomeInvalido);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(EmailObrigatorio)
                .EmailAddress().WithMessage(EmailInvalido);

            RuleFor(user => user.SenhaHash)
                .NotEmpty().WithMessage(SenhaObrigatoria)
                .Must(s => s.StartsWith("$2")).WithMessage(SenhaInvalida);
        }
    }
}
