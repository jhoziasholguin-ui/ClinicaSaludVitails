using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaSaludVitails.Migrations
{
    /// <inheritdoc />
    public partial class DniUnicoPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Hora",
                table: "Citas",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_DNI",
                table: "Pacientes",
                column: "DNI",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pacientes_DNI",
                table: "Pacientes");

            migrationBuilder.AlterColumn<string>(
                name: "Hora",
                table: "Citas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
