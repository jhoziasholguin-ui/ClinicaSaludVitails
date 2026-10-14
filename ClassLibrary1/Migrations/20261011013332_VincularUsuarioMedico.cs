using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaSaludVitails.Migrations
{
    /// <inheritdoc />
    public partial class VincularUsuarioMedico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicoId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_MedicoId",
                table: "Usuarios",
                column: "MedicoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Medicos_MedicoId",
                table: "Usuarios",
                column: "MedicoId",
                principalTable: "Medicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Medicos_MedicoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_MedicoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "MedicoId",
                table: "Usuarios");
        }
    }
}
