using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

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

        public void Alterar(Usuario usuario)
        {
            _context.Update(usuario);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
