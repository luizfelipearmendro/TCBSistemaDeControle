using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCBSistemaDeControle.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroFuncionarios",
                table: "Setores",
                newName: "UsuarioId");

            migrationBuilder.AlterColumn<string>(
                name: "Ativo",
                table: "Setores",
                type: "nvarchar(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Setores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Ativo",
                table: "Funcionarios",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Setores");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Funcionarios");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Setores",
                newName: "NumeroFuncionarios");

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Setores",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)");
        }
    }
}
