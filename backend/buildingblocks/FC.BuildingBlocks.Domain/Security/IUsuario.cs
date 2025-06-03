namespace FC.BuildingBlocks.Domain.Security
{
    public interface IUsuario
    {
        public Guid Id { get; }
        public string Email { get; }
    }
}
