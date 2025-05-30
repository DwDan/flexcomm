using FluentValidation;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommandValidator : AbstractValidator<CriarUsuarioCommand>
    {
        public static string NomeObrigatorio => "O nome é obrigatório.";
        public static string EmailObrigatorio => "O e-mail é obrigatório.";
        public static string SenhaObrigatoria => "A senha é obrigatória.";

        public static string NomeInvalido => "O nome deve ter no mínimo 3 caracteres.";
        public static string EmailInvalido => "O e-mail é inválido.";
        public static string SenhaTamanhoCaracteres => "A senha deve ter no máximo 20 caracteres.";
        public static string SenhaLetraMaiuscula => "A senha deve conter pelo menos uma letra maiúscula.";
        public static string SenhaLetraMinuscula => "A senha deve conter pelo menos uma letra minúscula.";
        public static string SenhaNumero => "A senha deve conter pelo menos um número.";
        public static string SenhaCaracter => "A senha deve conter pelo menos um caractere especial.";

        public CriarUsuarioCommandValidator()
        {
            RuleFor(user => user.Nome)
                .NotEmpty().WithMessage(NomeObrigatorio)
                .MinimumLength(3).WithMessage(NomeInvalido);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(EmailObrigatorio)
                .EmailAddress().WithMessage(EmailInvalido);

            RuleFor(user => user.Senha)
                .NotEmpty().WithMessage(SenhaObrigatoria)
                .Must(senha => senha != null && senha.Length >= 8 && senha.Length <= 20).WithMessage(SenhaTamanhoCaracteres)
                .Matches(@"[A-Z]").WithMessage(SenhaLetraMaiuscula)
                .Matches(@"[a-z]").WithMessage(SenhaLetraMinuscula)
                .Matches(@"\d").WithMessage(SenhaNumero)
                .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage(SenhaCaracter);
        }
    }
}
