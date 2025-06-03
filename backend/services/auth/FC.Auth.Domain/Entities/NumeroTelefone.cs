namespace FC.Auth.Domain.Entities
{
    public record NumeroTelefone(string Ddd, string Numero)
    {
        public string Ddd { get; private set; } = Ddd;
        public string Numero { get; private set; } = Numero;
        protected NumeroTelefone() : this(string.Empty, string.Empty) { }
    }
}
