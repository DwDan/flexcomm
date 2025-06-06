using FC.Auth.Domain.Entities;
using FluentValidation;

namespace FC.Auth.Domain.Validation
{
    public class UsuarioCriacaoValidation : AbstractValidator<Usuario>
    {
        public UsuarioCriacaoValidation()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithErrorCode("CriarUsuario.NomeObrigatorio")
                .MinimumLength(3).WithErrorCode("CriarUsuario.NomeInvalido");

            RuleFor(user => user.Email)
                .NotEmpty().WithErrorCode("CriarUsuario.EmailObrigatorio")
                .EmailAddress().WithErrorCode("CriarUsuario.EmailInvalido");

            RuleFor(user => user.SenhaHash)
                .NotEmpty().WithErrorCode("CriarUsuario.SenhaObrigatoria")
                .Must(s => s.StartsWith("$2")).WithErrorCode("CriarUsuario.SenhaInvalida");
        }
    }
}
