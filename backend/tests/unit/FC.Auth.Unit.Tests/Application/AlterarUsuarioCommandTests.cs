using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.AlterarUsuario.DTO;
using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Repositories;
using FC.Auth.Domain.Validation;
using FluentValidation;
using NSubstitute;

namespace FC.Auth.Unit.Tests.Application
{
    public class AlterarUsuarioCommandTests
    {
        private readonly AlterarUsuarioCommandHandler _handler;
        private readonly IUsuarioRepository _repositorio;

        public AlterarUsuarioCommandTests()
        {
            _repositorio = Substitute.For<IUsuarioRepository>();
            _handler = new AlterarUsuarioCommandHandler(_repositorio);
        }

        [Fact(DisplayName = "Alterar usuário válido deve executar com sucesso")]
        [Trait("Autenticação", "AlterarUsuarioCommand")]
        public async Task AlterarUsuario_Valido_DeveExecutarComSucesso()
        {
            // Arrange
            var usuario = new Usuario("teste", "teste@teste.com");
            var command = new AlterarUsuarioCommand() 
            { 
                Id = usuario.Id, 
                Endereco = new AlterarUsuarioEnderecoDto() 
                { 
                    Bairro = "Bairro Teste",
                    Cidade = "Cidade Teste",
                    Estado = "Estado Teste",
                    Numero = "123",
                    Logradouro = "Logradouro Teste",
                    Cep = "12345-678"
                },
                NomeCompleto = new AlterarUsuarioNomeCompletoDto() 
                { 
                    PrimeiroNome = "Primeiro Nome Teste", 
                    UltimoNome = "Ultimo Nome Teste"
                },
                Telefone = new AlterarUsuarioNumeroTelefoneDto() 
                { 
                    Ddd = "11", 
                    Numero = "987654321"
                }
            };

            _repositorio.ObterPorIdAsync(usuario.Id, CancellationToken.None).Returns(usuario);
            _repositorio.UnitOfWork.CommitAsync(CancellationToken.None).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _repositorio.Received(1).Alterar(Arg.Any<Usuario>());
            await _repositorio.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
        }

        [Fact(DisplayName = "Alterar usuário inválido deve disparar exceção")]
        [Trait("Autenticação", "AlterarUsuarioCommand")]
        public async Task AlterarUsuario_Invalido_DeveDispararExcecao()
        {
            // Arrange
            var usuario = new Usuario("", "");
            var command = new AlterarUsuarioCommand() 
            { 
                Id = usuario.Id, 
                Endereco = new AlterarUsuarioEnderecoDto(),
                NomeCompleto = new AlterarUsuarioNomeCompletoDto(),
                Telefone = new AlterarUsuarioNumeroTelefoneDto()
            };

            // Act & Assert
            _repositorio.ObterPorIdAsync(usuario.Id, CancellationToken.None).Returns(usuario);
            _repositorio.UnitOfWork.CommitAsync(CancellationToken.None).Returns(true);

            var result = await Assert.ThrowsAsync<ValidationException>(
                async () => await _handler.Handle(command, CancellationToken.None));

            Assert.Equal(15, result.Errors.Count());
            Assert.Contains("AlterarUsuario.NomeObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NomeInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.SobrenomeObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.SobrenomeInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.LogradouroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.BairroObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CidadeObrigatoria", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.EstadoObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CepObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.CepInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.DddObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.DddInvalido", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroTelefoneObrigatorio", result.Errors.Select(c => c.ErrorCode));
            Assert.Contains("AlterarUsuario.NumeroTelefoneInvalido", result.Errors.Select(c => c.ErrorCode));

            _repositorio.DidNotReceiveWithAnyArgs().Alterar(usuario);
            await _repositorio.UnitOfWork.DidNotReceiveWithAnyArgs().CommitAsync(CancellationToken.None);
        }
    }
}
