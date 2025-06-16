using FluentValidation;

namespace FC.Auth.Application.Usuarios.InativarUsuario
{
    public class InativarUsuarioCommandHandlerValidator : AbstractValidator<InativarUsuarioCommand>
    {
        public InativarUsuarioCommandHandlerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithErrorCode(InativarUsuario_IdInvalido);
        }

        public const string InativarUsuario_IdInvalido = "InativarUsuario.IdInvalido";
    }
}
