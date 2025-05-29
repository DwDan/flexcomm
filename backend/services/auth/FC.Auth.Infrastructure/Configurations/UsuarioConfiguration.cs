using FC.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FC.Auth.Infrastructure.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.SenhaHash)
                .HasColumnName("senha_hash")
                .HasColumnType("varchar")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("boolean")
                .IsRequired();

            builder.Property(u => u.Perfil)
                .HasColumnName("perfil")
                .HasColumnType("integer")
                .IsRequired();
        }
    }
}