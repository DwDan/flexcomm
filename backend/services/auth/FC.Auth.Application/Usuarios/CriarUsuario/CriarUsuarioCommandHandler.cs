using AutoMapper;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, bool>
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CriarUsuarioCommandHandler(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _repositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = _mapper.Map<Usuario>(request);

            var result = usuario.Validar();

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _repositorio.CriarAsync(usuario, cancellationToken);

            return await _repositorio.UnitOfWork.Commit();
        }
    }
}
