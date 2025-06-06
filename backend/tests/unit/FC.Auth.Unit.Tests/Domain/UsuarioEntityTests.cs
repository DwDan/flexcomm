using FC.Auth.Domain.Entities;
using FC.Auth.Domain.Validation;

namespace FC.Auth.Unit.Tests.Domain
{
    public class UsuarioEntityTests
    {
        [Fact(DisplayName = "Cria usuário deve criar usuário ativo")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void CriarUsuario_DeveCriar_UsuarioAtivo()
        {
            // Arrange
            var user = new Usuario("João", "joao@teste.com");
            user.DefinirSenhaCriptografada("hashed_password");

            // Act & Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("joao@teste.com", user.Email);
            Assert.Equal("João", user.NomeCompleto.PrimeiroNome);
            Assert.Equal("hashed_password", user.SenhaHash);
            Assert.True(user.Ativo);
        }

        [Fact(DisplayName = "Cria usuário deve criar usuário válido")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void CriarUsuario_DeveCriar_Valido()
        {
            // Arrange
            var user = new Usuario("João", "joao@teste.com");
            user.DefinirSenhaCriptografada("$2hashed_password");

            // Act
            var result = user.ValidarCriacao();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Cria usuário deve criar usuário inválido")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void CriarUsuario_DeveCriar_UsuarioInvalido()
        {
            // Arrange
            var user = new Usuario(string.Empty, string.Empty);
            user.DefinirSenhaCriptografada(string.Empty);

            // Act
            var result = user.ValidarCriacao();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_NomeObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_EmailObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_SenhaObrigatoria, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_NomeInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_EmailInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioCriacaoValidation.CriarUsuario_SenhaInvalida, result.Errors.Select(c => c.ErrorCode));
        }

        [Fact(DisplayName = "Alterar usuario deve alterar usuario valido com sucesso")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void AlterarUsuario_DeveAlterar_UsuarioValido_ComSucesso()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@teste.com");
            usuario.DefinirNomeCompleto("João", "Batista");
            usuario.DefinirEndereco("Rua A", "123", "Bairro B", "Cidade C", "Estado D", "12345678");
            usuario.DefinirTelefone("31", "987654321");

            // Act
            var result = usuario.ValidarAlteracao();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Alterar usuario deve impedir alteração usuário inválido")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void AlterarUsuario_ImpedirAlteracao_UsuarioInvalido()
        {
            // Arrange
            var usuario = new Usuario(string.Empty, string.Empty);
            usuario.DefinirNomeCompleto(string.Empty, string.Empty);
            usuario.DefinirEndereco(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            usuario.DefinirTelefone(string.Empty, string.Empty);

            // Act
            var result = usuario.ValidarAlteracao();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(15, result.Errors.Count);
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_NomeObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_NomeInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_SobrenomeObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_SobrenomeInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_LogradouroObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_NumeroObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_BairroObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_CidadeObrigatoria, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_EstadoObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_CepObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_CepInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_DddObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_DddInvalido, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_NumeroTelefoneObrigatorio, result.Errors.Select(c => c.ErrorCode));
            Assert.Contains(UsuarioAlteracaoValidation.AlterarUsuario_NumeroTelefoneInvalido, result.Errors.Select(c => c.ErrorCode));
        }


        [Fact(DisplayName = "Deve ser possível confirmar email com sucesso")]
        [Trait("Autenticação", "UsuarioEntity")]
        public void ConfirmacaoEmail_DeveRealizarComSucesso()
        {
            // Arrange
            var usuario = new Usuario("João", "joao@teste.com");

            // Act
            usuario.ConfirmarEmail();

            // Assert
            Assert.True(usuario.EmailConfirmado);
        }
    }
}
