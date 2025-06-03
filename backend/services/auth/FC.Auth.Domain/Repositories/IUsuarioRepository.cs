using FC.Auth.Domain.Entities;
using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void Criar(Usuario usuario);
        void Alterar(Usuario usuario);
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken);
        Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
