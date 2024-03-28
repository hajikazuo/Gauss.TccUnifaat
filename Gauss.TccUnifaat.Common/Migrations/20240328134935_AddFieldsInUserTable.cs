using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gauss.TccUnifaat.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "AspNetUsers",
                type: "datetime2(2)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExcluido",
                table: "AspNetUsers",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimaModificacao",
                table: "AspNetUsers",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataExcluido",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataUltimaModificacao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "AspNetUsers");
        }
    }
}
