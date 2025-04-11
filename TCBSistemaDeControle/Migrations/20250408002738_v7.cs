using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCBSistemaDeControle.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona a coluna CategoriaId à tabela Setores
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Setores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Cria a tabela Categorias
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(50)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            // Configura a relação entre Setores e Categorias
            migrationBuilder.CreateIndex(
                name: "IX_Setores_CategoriaId",
                table: "Setores",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setores_Categorias_CategoriaId",
                table: "Setores",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // Define a ação ao excluir uma categoria
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove a chave estrangeira e o índice antes de remover a coluna CategoriaId
            migrationBuilder.DropForeignKey(
                name: "FK_Setores_Categorias_CategoriaId",
                table: "Setores");

            migrationBuilder.DropIndex(
                name: "IX_Setores_CategoriaId",
                table: "Setores");

            // Remove a coluna CategoriaId da tabela Setores
            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Setores");

            // Remove a tabela Categorias
            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}