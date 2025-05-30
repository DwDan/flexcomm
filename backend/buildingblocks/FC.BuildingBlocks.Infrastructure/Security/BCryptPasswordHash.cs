using FC.BuildingBlocks.Domain;

namespace FC.BuildingBlocks.Infrastructure.Security
{
    public class BCryptPasswordHash : IPasswordHash
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
