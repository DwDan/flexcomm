using FC.Auth.Domain.Entities;
using FC.BuildingBlocks.Domain;

namespace FC.Auth.Domain.Repositories
{
    public interface IUsuarioRepositorio : IRepository<Usuario>
    {
        void Criar(Usuario usuario);
    }
}
