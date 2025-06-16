using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using MediatR;

namespace FC.Auth.Application.Usuarios.InativarUsuario
{
    public class InativarUsuarioCommandHandler : IRequestHandler<InativarUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _repositorio;

        public InativarUsuarioCommandHandler(IUsuarioRepository repository)
        {
            _repositorio = repository;
        }

        public async Task<bool> Handle(InativarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _repositorio.ObterPorIdAsync(request.Id, cancellationToken);

            if (usuario is null)
                throw new BusinessException(InativarUsuario_UsuarioNaoEncontrado);

            usuario.MarcarComoInativo();

            _repositorio.Alterar(usuario);

            return await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
        }

        public static string InativarUsuario_UsuarioNaoEncontrado = "InativarUsuario.UsuarioNaoEncontrado";
    }
}