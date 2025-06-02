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

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.SenhaHash)
                .HasColumnName("senha_hash")
                .HasColumnType("varchar")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("boolean")
                .IsRequired();

            builder.OwnsOne(u => u.NomeCompleto, nome =>
            {
                nome.Property(n => n.PrimeiroNome)
                    .HasColumnName("primeiro_nome")
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired();

                nome.Property(n => n.UltimoNome)
                    .HasColumnName("ultimo_nome")
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired(false);
            });

            builder.OwnsOne(u => u.Endereco, endereco =>
            {
                endereco.Property(e => e.Logradouro)
                    .HasColumnName("endereco_logradouro")
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired(false);

                endereco.Property(e => e.Numero)
                    .HasColumnName("endereco_numero")
                    .HasColumnType("varchar")
                    .HasMaxLength(10)
                    .IsRequired(false);

                endereco.Property(e => e.Bairro)
                    .HasColumnName("endereco_bairro")
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired(false);

                endereco.Property(e => e.Cidade)
                    .HasColumnName("endereco_cidade")
                    .HasColumnType("varchar")
                    .HasMaxLength(50)
                    .IsRequired(false);

                endereco.Property(e => e.Estado)
                    .HasColumnName("endereco_estado")
                    .HasColumnType("varchar")
                    .HasMaxLength(2)
                    .IsRequired(false);

                endereco.Property(e => e.Cep)
                    .HasColumnName("endereco_cep")
                    .HasColumnType("varchar")
                    .HasMaxLength(9)
                    .IsRequired(false);
            });

            builder.OwnsOne(u => u.Telefone, telefone =>
            {
                telefone.Property(t => t.Ddd)
                    .HasColumnName("telefone_ddd")
                    .HasColumnType("varchar")
                    .HasMaxLength(2)
                    .IsRequired(false);

                telefone.Property(t => t.Numero)
                    .HasColumnName("telefone_numero")
                    .HasColumnType("varchar")
                    .HasMaxLength(9)
                    .IsRequired(false);
            });
        }
    }
}