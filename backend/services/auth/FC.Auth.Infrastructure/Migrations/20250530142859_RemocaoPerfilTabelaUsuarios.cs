using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FC.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemocaoPerfilTabelaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "perfil",
                table: "usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "perfil",
                table: "usuarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
