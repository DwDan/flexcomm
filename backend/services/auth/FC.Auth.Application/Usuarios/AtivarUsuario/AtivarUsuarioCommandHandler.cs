using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using MediatR;

namespace FC.Auth.Application.Usuarios.AtivarUsuario
{
    public class AtivarUsuarioCommandHandler : IRequestHandler<AtivarUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _repositorio;

        public AtivarUsuarioCommandHandler(IUsuarioRepository repository)
        {
            _repositorio = repository;
        }

        public async Task<bool> Handle(AtivarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _repositorio.ObterPorIdAsync(request.Id, cancellationToken);

            if (usuario is null)
                throw new BusinessException(AtivarUsuario_UsuarioNaoEncontrado);

            usuario.MarcarComoAtivo();

            _repositorio.Alterar(usuario);

            return await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
        }

        public static string AtivarUsuario_UsuarioNaoEncontrado = "AtivarUsuario.UsuarioNaoEncontrado";
    }
}
