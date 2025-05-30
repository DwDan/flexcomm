namespace FC.BuildingBlocks.Domain.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IUsuario user);
    }
}
