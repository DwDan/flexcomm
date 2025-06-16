using FC.Auth.Application.Usuarios.AtivarUsuario;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.BuildingBlocks.Core.Exception;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class AtivarUsuarioCommandHandlerTests
    {
        private readonly AtivarUsuarioCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;

        public AtivarUsuarioCommandHandlerTests()
        {
            _repositorio = NSubstitute.Substitute.For<IUsuarioRepository>();
            _handler = new AtivarUsuarioCommandHandler(_repositorio);
        }

        [Fact(DisplayName = "Ativar usuario deve ativar com sucesso")]
        [Trait("Autenticação", "AtivarUsuarioCommandHandler")]
        public async Task AtivarUsuario_DeveAtivar_ComSucesso()
        {
            // Arrange
            var command = new AtivarUsuarioCommand(Guid.NewGuid());
            var usuario = new Usuario("Teste", "teste@test.com");
            usuario.MarcarComoInativo();
            usuario.LimparEventosDominio();

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

        [Fact(DisplayName = "Ativar usuario deve lançar exceção quando usuário não encontrado")]
        [Trait("Autenticação", "AtivarUsuarioCommandHandler")]
        public async Task AtivarUsuario_DeveLancarExcecao_QuandoUsuarioNaoEncontrado()
        {
            // Arrange
            var command = new AtivarUsuarioCommand(Guid.NewGuid());

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.NotNull(result);
            Assert.Contains(AtivarUsuarioCommandHandler.AtivarUsuario_UsuarioNaoEncontrado, result.Message);

            _repositorio.DidNotReceiveWithAnyArgs().Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(CancellationToken.None);
        }
    }
}
