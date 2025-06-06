using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FC.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfirmacaoEmailUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmado",
                table: "usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmado",
                table: "usuarios");
        }
    }
}
