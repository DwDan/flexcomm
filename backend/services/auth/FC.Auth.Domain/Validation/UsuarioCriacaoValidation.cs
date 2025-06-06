using FC.Auth.Domain.Entities;
using FluentValidation;

namespace FC.Auth.Domain.Validation
{
    public class UsuarioCriacaoValidation : AbstractValidator<Usuario>
    {
        public UsuarioCriacaoValidation()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithErrorCode(CriarUsuario_NomeObrigatorio)
                .MinimumLength(3).WithErrorCode(CriarUsuario_NomeInvalido);

            RuleFor(user => user.Email)
                .NotEmpty().WithErrorCode(CriarUsuario_EmailObrigatorio)
                .EmailAddress().WithErrorCode(CriarUsuario_EmailInvalido);

            RuleFor(user => user.SenhaHash)
                .NotEmpty().WithErrorCode(CriarUsuario_SenhaObrigatoria)
                .Must(s => s.StartsWith("$2")).WithErrorCode(CriarUsuario_SenhaInvalida);
        }

        public static string CriarUsuario_NomeObrigatorio => "CriarUsuario.NomeObrigatorio";
        public static string CriarUsuario_NomeInvalido => "CriarUsuario.NomeInvalido";
        public static string CriarUsuario_EmailObrigatorio => "CriarUsuario.EmailObrigatorio";
        public static string CriarUsuario_EmailInvalido => "CriarUsuario.EmailInvalido";
        public static string CriarUsuario_SenhaObrigatoria => "CriarUsuario.SenhaObrigatoria";
        public static string CriarUsuario_SenhaInvalida => "CriarUsuario.SenhaInvalida";
    }
}
