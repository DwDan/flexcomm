using System.Reflection;
using FC.Auth.Domain.Entities;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FC.Auth.Infrastructure
{
    public class AutenticacaoContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public AutenticacaoContext(DbContextOptions<AutenticacaoContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            var sucesso = await base.SaveChangesAsync(cancellationToken) > 0;

            if (sucesso)
                await DespacharEventosDominioAsync(cancellationToken);

            return sucesso;
        }

        public async Task EnsureCommitAsync(CancellationToken cancellationToken = default)
        {
            var success = await CommitAsync(cancellationToken);
            if (!success)
                throw new PersistenceException("Erro ao persistir dados de usuário.");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        private async Task DespacharEventosDominioAsync(CancellationToken cancellationToken)
        {
            var entidadesComEventos = ChangeTracker.Entries<IEntity>()
                .Where(e => e.Entity.ObterEventosDominio().Any())
                .Select(e => e.Entity)
                .ToList();

            var eventos = entidadesComEventos
                .SelectMany(e => e.ObterEventosDominio())
                .ToList();

            entidadesComEventos.ForEach(e => e.LimparEventosDominio());

            foreach (var domainEvent in eventos)
                await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
