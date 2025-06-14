using FC.Auth.Application;
using FC.Auth.Infrastructure;
using FC.Auth.WebAPI.Middlewares;
using FC.BuildingBlocks.Application;
using FC.BuildingBlocks.Infrastructure.Configuration;
using FC.BuildingBlocks.WebAPI;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FC.Auth.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração da aplicação
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            builder.Services.EnsureSettings<AplicacaoSettings, AplicacaoSettingsValidator>(builder.Configuration, "Application");

            // Serilog
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration));

            // Serviços principais
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Localização
            builder.Services.AddLocalization();
            builder.Services.ConfigureLocalization();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Injeção de dependências da aplicação
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            var app = builder.Build();

            // Middlewares e pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                ApplyMigrations(app);
            }

            app.UseSerilogRequestLogging();

            app.UseRequestLocalization();
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<LocalizedValidationExceptionMiddleware>();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AutenticacaoContext>();
            context.Database.Migrate();
        }
    }
}