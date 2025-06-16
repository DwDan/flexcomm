using FC.Auth.Application.Usuarios.InativarUsuario;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class InativarUsuarioCommandHandlerTests
    {
        private readonly InativarUsuarioCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;

        public InativarUsuarioCommandHandlerTests()
        {
            _repositorio = NSubstitute.Substitute.For<IUsuarioRepository>();
            _handler = new InativarUsuarioCommandHandler(_repositorio);
        }

        [Fact(DisplayName = "Inativar usuario deve inativar com sucesso")]
        [Trait("Autenticação", "InativarUsuarioCommandHandler")]
        public async Task InativarUsuario_DeveInativar_ComSucesso()
        {
            // Arrange
            var command = new InativarUsuarioCommand(Guid.NewGuid());
            var usuario = new Usuario("Teste", "teste@test.com");

            _repositorio.ObterPorIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(usuario);

            _repositorio.UnitOfWork.CommitAsync(Arg.Any<CancellationToken>())
                .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _repositorio.Received(1).Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
        }

        [Fact(DisplayName = "Inativar usuario deve lançar exceção quando usuário não encontrado")]
        [Trait("Autenticação", "InativarUsuarioCommandHandler")]
        public async Task InativarUsuario_DeveLancarExcecao_QuandoUsuarioNaoEncontrado()
        {
            // Arrange
            var command = new InativarUsuarioCommand(Guid.NewGuid());

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.NotNull(result);
            Assert.Contains(InativarUsuarioCommandHandler.InativarUsuario_UsuarioNaoEncontrado, result.Message);

            _repositorio.DidNotReceiveWithAnyArgs().Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(CancellationToken.None);
        }
    }
}
