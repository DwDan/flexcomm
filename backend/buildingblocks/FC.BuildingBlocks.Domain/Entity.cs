namespace FC.BuildingBlocks.Domain
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }

        private readonly List<IDomainEvent> _eventosDominio = new();
        public IReadOnlyCollection<IDomainEvent> ObterEventosDominio() => _eventosDominio;
        public void AdicionarEventoDominio(IDomainEvent evento) => _eventosDominio.Add(evento);
        public void LimparEventosDominio() => _eventosDominio.Clear();
    }
}
