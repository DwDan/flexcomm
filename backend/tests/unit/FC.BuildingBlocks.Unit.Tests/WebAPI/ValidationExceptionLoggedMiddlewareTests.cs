using System.Net;
using FC.BuildingBlocks.Core.Exception;
using FC.BuildingBlocks.WebAPI;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.WebApi.Jwt;
using NSubstitute;

namespace FC.BuildingBlocks.Unit.WebAPI.Tests
{
    public class ValidationExceptionLoggedMiddlewareTests
    {
        private TestServer BuildServer(Exception exceptionToThrow)
        {
            return new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    var logger = Substitute.For<ILogger<ValidationExceptionLoggedMiddleware>>();
                    services.AddSingleton(logger);
                })
                .Configure(app =>
                {
                    app.UseMiddleware<ValidationExceptionLoggedMiddleware>();
                    app.Run(_ => throw exceptionToThrow);
                }));
        }

        [Fact]
        public async Task DeveRetornar400_ParaValidationException()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new("Campo", "Mensagem de erro") { ErrorCode = "Erro.Campo" }
            };

            var server = BuildServer(new ValidationException(failures));
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("error", content);
        }

        [Fact]
        public async Task DeveRetornar401_ParaInvalidCredentialsException()
        {
            var server = BuildServer(new InvalidCredentialsException("Credenciais inválidas"));
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar404_ParaNotFoundException()
        {
            var server = BuildServer(new NotFoundException("Não encontrado"));
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar500_ParaPersistenceException()
        {
            var server = BuildServer(new PersistenceException("Erro persistência"));
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar400_ParaBadRequestException()
        {
            var server = BuildServer(new BadRequestException("Erro.BadRequest"));
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeveRetornar500_ParaErroDesconhecido()
        {
            var server = BuildServer(new InvalidOperationException("Erro desconhecido"));
            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
