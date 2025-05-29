using System.Reflection;
using FC.Auth.Domain.Entities;
using FC.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace FC.Auth.Infrastructure
{
    public class AutenticacaoContext(DbContextOptions<AutenticacaoContext> options) : DbContext(options), IUnitOfWork
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            var result = base.SaveChangesAsync(cancellationToken);

            return await result > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
