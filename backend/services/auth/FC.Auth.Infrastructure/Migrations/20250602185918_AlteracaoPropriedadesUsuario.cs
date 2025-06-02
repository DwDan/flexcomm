using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FC.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoPropriedadesUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nome",
                table: "usuarios");

            migrationBuilder.AddColumn<string>(
                name: "endereco_bairro",
                table: "usuarios",
                type: "varchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_cep",
                table: "usuarios",
                type: "varchar",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_cidade",
                table: "usuarios",
                type: "varchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_estado",
                table: "usuarios",
                type: "varchar",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_logradouro",
                table: "usuarios",
                type: "varchar",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco_numero",
                table: "usuarios",
                type: "varchar",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primeiro_nome",
                table: "usuarios",
                type: "varchar",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "telefone_ddd",
                table: "usuarios",
                type: "varchar",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefone_numero",
                table: "usuarios",
                type: "varchar",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ultimo_nome",
                table: "usuarios",
                type: "varchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuarios_email",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_bairro",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_cep",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_cidade",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_estado",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_logradouro",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "endereco_numero",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "primeiro_nome",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "telefone_ddd",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "telefone_numero",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "ultimo_nome",
                table: "usuarios");

            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "usuarios",
                type: "varchar",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
