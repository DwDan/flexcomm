using FluentValidation;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommandValidator : AbstractValidator<AlterarUsuarioCommand>
    {
        public static string NomeObrigatorio => "O primeiro nome é obrigatório.";
        public static string NomeInvalido => "O primeiro nome deve ter no mínimo 3 caracteres.";

        public static string SobrenomeObrigatorio => "O sobrenome é obrigatório.";
        public static string SobrenomeInvalido => "O sobrenome deve ter no mínimo 2 caracteres.";

        public static string LogradouroObrigatorio => "O logradouro é obrigatório.";
        public static string NumeroObrigatorio => "O número é obrigatório.";
        public static string BairroObrigatorio => "O bairro é obrigatório.";
        public static string CidadeObrigatoria => "A cidade é obrigatória.";
        public static string EstadoObrigatorio => "O estado é obrigatório.";
        public static string CepObrigatorio => "O CEP é obrigatório.";
        public static string CepInvalido => "O CEP está em formato inválido.";

        public static string DddObrigatorio => "O DDD é obrigatório.";
        public static string DddInvalido => "O DDD deve conter exatamente 2 dígitos.";

        public static string NumeroTelefoneObrigatorio => "O número de telefone é obrigatório.";
        public static string NumeroTelefoneInvalido => "O número de telefone deve conter entre 8 e 9 dígitos.";

        public AlterarUsuarioCommandValidator()
        {
            RuleFor(user => user.NomeCompleto.PrimeiroNome)
                .NotEmpty().WithMessage(NomeObrigatorio)
                .MinimumLength(3).WithMessage(NomeInvalido);

            RuleFor(user => user.NomeCompleto.UltimoNome)
                .NotEmpty().WithMessage(SobrenomeObrigatorio)
                .MinimumLength(2).WithMessage(SobrenomeInvalido);

            When(user => user.Endereco != null, () =>
            {
                RuleFor(user => user.Endereco!.Logradouro)
                    .NotEmpty().WithMessage(LogradouroObrigatorio);

                RuleFor(user => user.Endereco!.Numero)
                    .NotEmpty().WithMessage(NumeroObrigatorio);

                RuleFor(user => user.Endereco!.Bairro)
                    .NotEmpty().WithMessage(BairroObrigatorio);

                RuleFor(user => user.Endereco!.Cidade)
                    .NotEmpty().WithMessage(CidadeObrigatoria);

                RuleFor(user => user.Endereco!.Estado)
                    .NotEmpty().WithMessage(EstadoObrigatorio);

                RuleFor(user => user.Endereco!.Cep)
                    .NotEmpty().WithMessage(CepObrigatorio)
                    .Matches(@"^\d{5}-?\d{3}$").WithMessage(CepInvalido);
            });

            When(user => user.Telefone != null, () =>
            {
                RuleFor(user => user.Telefone!.Ddd)
                    .NotEmpty().WithMessage(DddObrigatorio)
                    .Matches(@"^\d{2}$").WithMessage(DddInvalido);

                RuleFor(user => user.Telefone!.Numero)
                    .NotEmpty().WithMessage(NumeroTelefoneObrigatorio)
                    .Matches(@"^\d{8,9}$").WithMessage(NumeroTelefoneInvalido);
            });
        }
    }
}
