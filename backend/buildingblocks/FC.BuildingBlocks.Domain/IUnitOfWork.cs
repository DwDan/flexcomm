namespace FC.BuildingBlocks.Domain
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
    }
}