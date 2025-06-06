using FluentValidation;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommandValidator : AbstractValidator<CriarUsuarioCommand>
    {
        public CriarUsuarioCommandValidator()
        {
            RuleFor(user => user.Nome)
                .NotEmpty().WithErrorCode("CriarUsuario.NomeObrigatorio")
                .MinimumLength(3).WithErrorCode("CriarUsuario.NomeInvalido");

            RuleFor(user => user.Email)
                .NotEmpty().WithErrorCode("CriarUsuario.EmailObrigatorio")
                .EmailAddress().WithErrorCode("CriarUsuario.EmailInvalido");

            RuleFor(user => user.Senha)
                .NotEmpty().WithErrorCode("CriarUsuario.SenhaObrigatoria")
                .Must(senha => senha != null && senha.Length >= 8 && senha.Length <= 20).WithErrorCode("CriarUsuario.SenhaTamanhoCaracteres")
                .Matches(@"[A-Z]").WithErrorCode("CriarUsuario.SenhaLetraMaiuscula")
                .Matches(@"[a-z]").WithErrorCode("CriarUsuario.SenhaLetraMinuscula")
                .Matches(@"\d").WithErrorCode("CriarUsuario.SenhaNumero")
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithErrorCode("CriarUsuario.SenhaCaracter");
        }
    }
}
