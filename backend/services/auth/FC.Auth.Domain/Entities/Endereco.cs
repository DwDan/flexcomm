namespace FC.Auth.Domain.Entities
{
    public record Endereco(string Logradouro, string Numero, string Bairro, string Cidade, string Estado, string Cep)
    {
        public string Logradouro { get; private set; } = Logradouro;
        public string Numero { get; private set; } = Numero;
        public string Bairro { get; private set; } = Bairro;
        public string Cidade { get; private set; } = Cidade;
        public string Estado { get; private set; } = Estado;
        public string Cep { get; private set; } = Cep;

        protected Endereco() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }
    }
}
