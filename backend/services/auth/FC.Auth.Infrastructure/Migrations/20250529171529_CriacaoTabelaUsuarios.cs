using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FC.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoTabelaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar", maxLength: 150, nullable: false),
                    senha_hash = table.Column<string>(type: "varchar", maxLength: 256, nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    perfil = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
