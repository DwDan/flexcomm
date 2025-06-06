using FluentValidation;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommandValidator : AbstractValidator<CriarUsuarioCommand>
    {
        public CriarUsuarioCommandValidator()
        {
            RuleFor(user => user.Nome)
                .NotEmpty().WithErrorCode(CriarUsuario_NomeObrigatorio)
                .MinimumLength(3).WithErrorCode(CriarUsuario_NomeInvalido);

            RuleFor(user => user.Email)
                .NotEmpty().WithErrorCode(CriarUsuario_EmailObrigatorio)
                .EmailAddress().WithErrorCode(CriarUsuario_EmailInvalido);

            RuleFor(user => user.Senha)
                .NotEmpty().WithErrorCode(CriarUsuario_SenhaObrigatoria)
                .Must(senha => senha != null && senha.Length >= 8 && senha.Length <= 20).WithErrorCode(CriarUsuario_SenhaTamanhoCaracteres)
                .Matches(@"[A-Z]").WithErrorCode(CriarUsuario_SenhaLetraMaiuscula)
                .Matches(@"[a-z]").WithErrorCode(CriarUsuario_SenhaLetraMinuscula)
                .Matches(@"\d").WithErrorCode(CriarUsuario_SenhaNumero)
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithErrorCode(CriarUsuario_SenhaCaracter);
        }

        public static string CriarUsuario_NomeObrigatorio => "CriarUsuario.NomeObrigatorio";
        public static string CriarUsuario_NomeInvalido => "CriarUsuario.NomeInvalido";
        public static string CriarUsuario_EmailObrigatorio => "CriarUsuario.EmailObrigatorio";
        public static string CriarUsuario_EmailInvalido => "CriarUsuario.EmailInvalido";
        public static string CriarUsuario_SenhaObrigatoria => "CriarUsuario.SenhaObrigatoria";
        public static string CriarUsuario_SenhaTamanhoCaracteres => "CriarUsuario.SenhaTamanhoCaracteres";
        public static string CriarUsuario_SenhaLetraMaiuscula => "CriarUsuario.SenhaLetraMaiuscula";
        public static string CriarUsuario_SenhaLetraMinuscula => "CriarUsuario.SenhaLetraMinuscula";
        public static string CriarUsuario_SenhaNumero => "CriarUsuario.SenhaNumero";
        public static string CriarUsuario_SenhaCaracter => "CriarUsuario.SenhaCaracter";
    }
}
