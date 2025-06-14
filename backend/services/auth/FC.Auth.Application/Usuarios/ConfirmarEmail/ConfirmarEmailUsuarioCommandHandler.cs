using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain.Security;
using MediatR;

namespace FC.Auth.Application.Usuarios.ConfirmarEmail
{
    public class ConfirmarEmailUsuarioCommandHandler : IRequestHandler<ConfirmarEmailUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _repositorio;
        private readonly IEmailConfirmationTokenValidator _tokenValidator;
        public ConfirmarEmailUsuarioCommandHandler(IUsuarioRepository repositorio, IEmailConfirmationTokenValidator tokenValidator)
        {
            _repositorio = repositorio;
            _tokenValidator = tokenValidator;
        }

        public async Task<bool> Handle(ConfirmarEmailUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioId = _tokenValidator.Validate(request.Token);

            var usuario = await _repositorio.ObterPorIdAsync(usuarioId, cancellationToken);
            if (usuario is null)
                throw new NotFoundException("ConfirmacaoEmail.UsuarioNaoEncontrado");

            usuario.ConfirmarEmail();
            _repositorio.Alterar(usuario);
            return await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}