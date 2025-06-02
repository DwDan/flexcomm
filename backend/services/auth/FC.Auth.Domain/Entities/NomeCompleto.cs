namespace FC.Auth.Domain.Entities
{
    public record NomeCompleto(string PrimeiroNome, string UltimoNome)
    {
        public string PrimeiroNome { get; private set; } = PrimeiroNome;
        public string UltimoNome { get; private set; } = UltimoNome;

        protected NomeCompleto() : this(string.Empty, string.Empty) { }
    }
}
