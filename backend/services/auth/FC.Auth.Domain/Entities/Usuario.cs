using FC.Auth.Domain.Messaging.Events;
using FC.Auth.Domain.Validation;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;
using FluentValidation.Results;

namespace FC.Auth.Domain.Entities
{
    public class Usuario : Entity, IUsuario, IAggregateRoot
    {
        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public bool Ativo { get; private set; }
        public bool EmailConfirmado { get; private set; }

        public NomeCompleto NomeCompleto { get; private set; }
        public Endereco? Endereco { get; private set; }
        public NumeroTelefone? Telefone { get; private set; }

        protected Usuario() { }

        public Usuario(string nome, string email)
        {
            Id = Guid.NewGuid();
            NomeCompleto = new NomeCompleto(nome, string.Empty);
            Email = email;
            Ativo = true;
            SenhaHash = string.Empty;
        }

        public ValidationResult ValidarCriacao()
        {
            return new UsuarioCriacaoValidation().Validate(this);
        }

        public void DefinirSenhaCriptografada(string senhaCriptografada)
        {
            SenhaHash = senhaCriptografada;
        }

        public void DefinirEndereco(string logradouro, string numero, string bairro, string cidade, string estado, string cep)
        {
            Endereco = new Endereco(logradouro, numero, bairro, cidade, estado, cep);
        }

        public void DefinirNomeCompleto(string primeiroNome, string ultimoNome)
        {
            NomeCompleto = new NomeCompleto(primeiroNome, ultimoNome);
        }

        public void DefinirTelefone(string ddd, string numeroTelefone)
        {
            Telefone = new NumeroTelefone(ddd, numeroTelefone);
        }

        public ValidationResult ValidarAlteracao()
        {
            return new UsuarioAlteracaoValidation().Validate(this);
        }

        public void ConfirmarEmail()
        {
            EmailConfirmado = true;

            AdicionarEventoDominio(new EmailUsuarioConfirmadoEvent(NomeCompleto.PrimeiroNome, Email));
        }

        public void MarcarComoCriado()
        {
            AdicionarEventoDominio(new UsuarioCriadoEvent(Id, Email));
        }

        public void MarcarComoAlterado()
        {
            AdicionarEventoDominio(new UsuarioAlteradoEvent(Id, NomeCompleto.PrimeiroNome, Email));
        }
    }
}
