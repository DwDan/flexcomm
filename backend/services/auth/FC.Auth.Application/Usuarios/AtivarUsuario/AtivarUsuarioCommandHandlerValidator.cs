using FluentValidation;

namespace FC.Auth.Application.Usuarios.AtivarUsuario
{
    public class AtivarUsuarioCommandHandlerValidator : AbstractValidator<AtivarUsuarioCommand>
    {
        public AtivarUsuarioCommandHandlerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithErrorCode(AtivarUsuario_IdInvalido);
        }

        public const string AtivarUsuario_IdInvalido = "AtivarUsuario.IdInvalido";
    }
}
