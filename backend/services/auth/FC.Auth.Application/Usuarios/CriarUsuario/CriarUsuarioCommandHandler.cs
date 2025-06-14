using AutoMapper;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
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
            IPasswordHash passwordHasher,
            IEventBusProducer eventProducer)
        {
            _repositorio = usuarioRepositorio;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuarioExistente = await _repositorio.ObterPorEmailAsync(request.Email, cancellationToken);
            if(usuarioExistente is not null)
                throw new BusinessException($"CriarUsuario.EmailDuplicado");

            var usuario = _mapper.Map<Usuario>(request);
            var senhaCriptografada = _passwordHasher.HashPassword(request.Senha);
            usuario.DefinirSenhaCriptografada(senhaCriptografada);

            var result = usuario.ValidarCriacao();

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            usuario.MarcarComoCriado();

            _repositorio.Criar(usuario);

            await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            return usuario.Id;
        }
    }
}
