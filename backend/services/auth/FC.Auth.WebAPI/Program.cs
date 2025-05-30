using FC.Auth.Application;
using FC.Auth.Domain.Repositories;
using FC.Auth.Infrastructure;
using FC.Auth.Infrastructure.Repositories;
using FC.BuildingBlocks.Application;
using FC.BuildingBlocks.WebAPI;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FC.BuildingBlocks.Infrastructure.Security;
using FC.BuildingBlocks.Domain;
using FC.BuildingBlocks.Domain.Security;

namespace FC.Auth.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            PresentationInitializer(builder);

            InfrastructureInitializer(builder);

            ApplicationInitializer(builder);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ValidationExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        public static void PresentationInitializer(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void InfrastructureInitializer(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<AutenticacaoContext>(options =>
                options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly("FC.Auth.Infrastructure")));

            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<AutenticacaoContext>());
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IPasswordHash, BCryptPasswordHash>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        }

        public static void ApplicationInitializer(WebApplicationBuilder builder)
        {
            var applicationAssembly = typeof(ApplicationLayer).Assembly;

            builder.Services.AddAutoMapper(applicationAssembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(applicationAssembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            builder.Services.AddValidatorsFromAssembly(applicationAssembly);
        }
    }
}
