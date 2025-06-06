using FluentValidation;

namespace FC.Auth.Application.Usuarios.ConfirmarEmail
{
    public class ConfirmarEmailUsuarioCommandValidator : AbstractValidator<ConfirmarEmailUsuarioCommand>
    {
        public ConfirmarEmailUsuarioCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithErrorCode(ConfirmacaoEmail_TokenObrigatorio);
        }

        public static string ConfirmacaoEmail_TokenObrigatorio => "ConfirmacaoEmail.TokenObrigatorio";
    }
}
