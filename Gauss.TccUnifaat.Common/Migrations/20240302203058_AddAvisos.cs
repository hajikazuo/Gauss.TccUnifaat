using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gauss.TccUnifaat.Migrations
{
    /// <inheritdoc />
    public partial class AddAvisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Turmas_TurmaId",
                table: "Disciplinas");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Presencas",
                newName: "PresencaId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TurmaId",
                table: "Disciplinas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Avisos",
                columns: table => new
                {
                    AvisoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataAviso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TurmaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Excluido = table.Column<bool>(type: "bit", nullable: false),
                    DataExcluido = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataUltimaModificacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avisos", x => x.AvisoId);
                    table.ForeignKey(
                        name: "FK_Avisos_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "TurmaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avisos_TurmaId",
                table: "Avisos",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Turmas_TurmaId",
                table: "Disciplinas",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "TurmaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Turmas_TurmaId",
                table: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Avisos");

            migrationBuilder.RenameColumn(
                name: "PresencaId",
                table: "Presencas",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "TurmaId",
                table: "Disciplinas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Turmas_TurmaId",
                table: "Disciplinas",
                column: "TurmaId",
                principalTable: "Turmas",
                principalColumn: "TurmaId");
        }
    }
}
