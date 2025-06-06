using FluentValidation;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommandValidator : AbstractValidator<AlterarUsuarioCommand>
    {
        public AlterarUsuarioCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithErrorCode("AlterarUsuario.IdObrigatorio");
        }
    }
}
