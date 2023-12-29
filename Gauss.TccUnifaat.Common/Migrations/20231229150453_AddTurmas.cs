using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gauss.TccUnifaat.Migrations
{
    /// <inheritdoc />
    public partial class AddTurmas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TurmaId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    TurmaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    DataExcluido = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    DataUltimaModificacao = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.TurmaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TurmaId",
                table: "AspNetUsers",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Turmas_TurmaId",
                table: "AspNetUsers",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "TurmaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Turmas_TurmaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Turmas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TurmaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "AspNetUsers");
        }
    }
}
