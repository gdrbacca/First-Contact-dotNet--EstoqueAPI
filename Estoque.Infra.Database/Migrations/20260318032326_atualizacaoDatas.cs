using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estoque.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class atualizacaoDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "produto_estoque",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QUANTIDADE_DISPONIVEL = table.Column<int>(type: "int", nullable: false),
                    ESTOQUE_MINIMO = table.Column<int>(type: "int", nullable: false),
                    ULTIMA_ATUALIZACAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produto_estoque", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movimento_estoque",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoEstoqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TIPO = table.Column<short>(type: "smallint", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "int", nullable: true),
                    PEDIDO_EXTERNO_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimento_estoque", x => x.id);
                    table.ForeignKey(
                        name: "FK_movimento_estoque_produto_estoque_ProdutoEstoqueId",
                        column: x => x.ProdutoEstoqueId,
                        principalTable: "produto_estoque",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movimento_estoque_ProdutoEstoqueId",
                table: "movimento_estoque",
                column: "ProdutoEstoqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movimento_estoque");

            migrationBuilder.DropTable(
                name: "produto_estoque");
        }
    }
}
