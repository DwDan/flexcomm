using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Domain;

namespace FC.Auth.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AutenticacaoContext _context;

        public UsuarioRepository(AutenticacaoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Criar(Usuario usuario)
        {
            _context.Add(usuario);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
