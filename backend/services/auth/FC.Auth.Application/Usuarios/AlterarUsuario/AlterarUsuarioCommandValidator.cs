using FluentValidation;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommandValidator : AbstractValidator<AlterarUsuarioCommand>
    {
        public AlterarUsuarioCommandValidator()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithErrorCode("AlterarUsuario.NomeObrigatorio")
                .MinimumLength(3).WithErrorCode("AlterarUsuario.NomeInvalido");

            RuleFor(user => user.NomeCompleto.UltimoNome)
                .NotEmpty().WithErrorCode("AlterarUsuario.SobrenomeObrigatorio")
                .MinimumLength(2).WithErrorCode("AlterarUsuario.SobrenomeInvalido");

            When(user => user.Endereco != null, () =>
            {
                RuleFor(user => user.Endereco!.Logradouro)
                    .NotEmpty().WithErrorCode("AlterarUsuario.LogradouroObrigatorio");

                RuleFor(user => user.Endereco!.Numero)
                    .NotEmpty().WithErrorCode("AlterarUsuario.NumeroObrigatorio");

                RuleFor(user => user.Endereco!.Bairro)
                    .NotEmpty().WithErrorCode("AlterarUsuario.BairroObrigatorio");

                RuleFor(user => user.Endereco!.Cidade)
                    .NotEmpty().WithErrorCode("AlterarUsuario.CidadeObrigatoria");

                RuleFor(user => user.Endereco!.Estado)
                    .NotEmpty().WithErrorCode("AlterarUsuario.EstadoObrigatorio");

                RuleFor(user => user.Endereco!.Cep)
                    .NotEmpty().WithErrorCode("AlterarUsuario.CepObrigatorio")
                    .Matches(@"^\d{5}-?\d{3}$").WithErrorCode("AlterarUsuario.CepInvalido");
            });

            When(user => user.Telefone != null, () =>
            {
                RuleFor(user => user.Telefone!.Ddd)
                    .NotEmpty().WithErrorCode("AlterarUsuario.DddObrigatorio")
                    .Matches(@"^\d{2}$").WithErrorCode("AlterarUsuario.DddInvalido");

                RuleFor(user => user.Telefone!.Numero)
                    .NotEmpty().WithErrorCode("AlterarUsuario.NumeroTelefoneObrigatorio")
                    .Matches(@"^\d{8,9}$").WithErrorCode("AlterarUsuario.NumeroTelefoneInvalido");
            });
        }
    }
}
