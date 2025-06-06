using FluentValidation;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommandValidator : AbstractValidator<AlterarUsuarioCommand>
    {
        public AlterarUsuarioCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithErrorCode(AlterarUsuario_IdObrigatorio);
        }

        public static string AlterarUsuario_IdObrigatorio => "AlterarUsuario.IdObrigatorio";
    }
}
