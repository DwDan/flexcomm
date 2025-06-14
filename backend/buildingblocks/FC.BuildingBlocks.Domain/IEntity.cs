namespace FC.BuildingBlocks.Domain
{
    public interface IEntity
    {
        IReadOnlyCollection<IDomainEvent> ObterEventosDominio();
        void AdicionarEventoDominio(IDomainEvent evento);
        void LimparEventosDominio();
    }
}
