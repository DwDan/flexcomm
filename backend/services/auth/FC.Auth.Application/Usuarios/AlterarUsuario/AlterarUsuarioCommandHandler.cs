using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using FluentValidation;
using MediatR;

namespace FC.Auth.Application.Usuarios.AlterarUsuario
{
    public class AlterarUsuarioCommandHandler : IRequestHandler<AlterarUsuarioCommand, bool>
    {
        private readonly IUsuarioRepository _repositorio;

        public AlterarUsuarioCommandHandler(IUsuarioRepository usuarioRepositorio)
        {
            _repositorio = usuarioRepositorio;
        }

        public async Task<bool> Handle(AlterarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _repositorio.ObterPorIdAsync(request.Id, cancellationToken);
            if (usuario == null)
                throw new BadRequestException("Usuário não encontrado.");

            usuario.DefinirNomeCompleto(request.NomeCompleto.PrimeiroNome, request.NomeCompleto.UltimoNome);

            if (request.Endereco is not null)
            {
                usuario.DefinirEndereco(
                    request.Endereco.Logradouro,
                    request.Endereco.Numero,
                    request.Endereco.Bairro,
                    request.Endereco.Cidade,
                    request.Endereco.Estado,
                    request.Endereco.Cep
                );
            }

            if (request.Telefone is not null)
            {
                usuario.DefinirTelefone(request.Telefone.Ddd, request.Telefone.Numero);
            }

            var result = usuario.ValidarAlteracao();
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _repositorio.Alterar(usuario);

            return await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
        }
    }
}
