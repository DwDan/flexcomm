using AutoMapper;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Domain;
using FluentValidation;
using MediatR;

namespace FC.Auth.Application.Usuarios.CriarUsuario
{
    public class CriarUsuarioCommandHandler : IRequestHandler<CriarUsuarioCommand, Guid>
    {
        private readonly IUsuarioRepository _repositorio;
        private readonly IMapper _mapper;
        private readonly IPasswordHash _passwordHasher;

        public CriarUsuarioCommandHandler(IUsuarioRepository usuarioRepositorio, 
            IMapper mapper, 
            IPasswordHash passwordHasher)
        {
            _repositorio = usuarioRepositorio;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = _mapper.Map<Usuario>(request);
            var senhaCriptografada = _passwordHasher.HashPassword(request.Senha);
            usuario.DefinirSenhaCriptografada(senhaCriptografada);

            var result = usuario.ValidarCriacao();

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _repositorio.Criar(usuario);

            await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            return usuario.Id;
        }
    }
}
