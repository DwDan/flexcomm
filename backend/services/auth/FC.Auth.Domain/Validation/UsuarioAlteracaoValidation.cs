using FC.Auth.Domain.Entities;
using FluentValidation;

namespace FC.Auth.Domain.Validation
{
    public class UsuarioAlteracaoValidation : AbstractValidator<Usuario>
    {
        public UsuarioAlteracaoValidation()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithErrorCode(AlterarUsuario_NomeObrigatorio)
                .MinimumLength(3).WithErrorCode(AlterarUsuario_NomeInvalido);

            RuleFor(user => user.NomeCompleto.UltimoNome)
                .NotEmpty().WithErrorCode(AlterarUsuario_SobrenomeObrigatorio)
                .MinimumLength(2).WithErrorCode(AlterarUsuario_SobrenomeInvalido);

            When(user => user.Endereco != null, () =>
            {
                RuleFor(user => user.Endereco!.Logradouro)
                    .NotEmpty().WithErrorCode(AlterarUsuario_LogradouroObrigatorio);

                RuleFor(user => user.Endereco!.Numero)
                    .NotEmpty().WithErrorCode(AlterarUsuario_NumeroObrigatorio);

                RuleFor(user => user.Endereco!.Bairro)
                    .NotEmpty().WithErrorCode(AlterarUsuario_BairroObrigatorio);

                RuleFor(user => user.Endereco!.Cidade)
                    .NotEmpty().WithErrorCode(AlterarUsuario_CidadeObrigatoria);

                RuleFor(user => user.Endereco!.Estado)
                    .NotEmpty().WithErrorCode(AlterarUsuario_EstadoObrigatorio);

                RuleFor(user => user.Endereco!.Cep)
                    .NotEmpty().WithErrorCode(AlterarUsuario_CepObrigatorio)
                    .Matches(@"^\d{5}-?\d{3}$").WithErrorCode(AlterarUsuario_CepInvalido);
            });

            When(user => user.Telefone != null, () =>
            {
                RuleFor(user => user.Telefone!.Ddd)
                    .NotEmpty().WithErrorCode(AlterarUsuario_DddObrigatorio)
                    .Matches(@"^\d{2}$").WithErrorCode(AlterarUsuario_DddInvalido);

                RuleFor(user => user.Telefone!.Numero)
                    .NotEmpty().WithErrorCode(AlterarUsuario_NumeroTelefoneObrigatorio)
                    .Matches(@"^\d{8,9}$").WithErrorCode(AlterarUsuario_NumeroTelefoneInvalido);
            });
        }

        public static string AlterarUsuario_NomeObrigatorio => "AlterarUsuario.NomeObrigatorio";
        public static string AlterarUsuario_NomeInvalido => "AlterarUsuario.NomeInvalido";
        public static string AlterarUsuario_SobrenomeObrigatorio => "AlterarUsuario.SobrenomeObrigatorio";
        public static string AlterarUsuario_SobrenomeInvalido => "AlterarUsuario.SobrenomeInvalido";
        public static string AlterarUsuario_LogradouroObrigatorio => "AlterarUsuario.LogradouroObrigatorio";
        public static string AlterarUsuario_NumeroObrigatorio => "AlterarUsuario.NumeroObrigatorio";
        public static string AlterarUsuario_BairroObrigatorio => "AlterarUsuario.BairroObrigatorio";
        public static string AlterarUsuario_CidadeObrigatoria => "AlterarUsuario.CidadeObrigatoria";
        public static string AlterarUsuario_EstadoObrigatorio => "AlterarUsuario.EstadoObrigatorio";
        public static string AlterarUsuario_CepObrigatorio => "AlterarUsuario.CepObrigatorio";
        public static string AlterarUsuario_CepInvalido => "AlterarUsuario.CepInvalido";
        public static string AlterarUsuario_DddObrigatorio => "AlterarUsuario.DddObrigatorio";
        public static string AlterarUsuario_DddInvalido => "AlterarUsuario.DddInvalido";
        public static string AlterarUsuario_NumeroTelefoneObrigatorio => "AlterarUsuario.NumeroTelefoneObrigatorio";
        public static string AlterarUsuario_NumeroTelefoneInvalido => "AlterarUsuario.NumeroTelefoneInvalido";
    }
}
