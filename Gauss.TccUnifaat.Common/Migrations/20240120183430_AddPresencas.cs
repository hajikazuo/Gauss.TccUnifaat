using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gauss.TccUnifaat.Migrations
{
    /// <inheritdoc />
    public partial class AddPresencas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Presencas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAula = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Presente = table.Column<bool>(type: "bit", nullable: false),
                    TurmaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    DataExcluido = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    DataUltimaModificacao = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presencas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presencas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presencas_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "TurmaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_TurmaId",
                table: "Presencas",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_UsuarioId",
                table: "Presencas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Presencas");
        }
    }
}
