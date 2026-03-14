using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadApp.Migrations
{
    /// <inheritdoc />
    public partial class FixAccionResponsableFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acciones_Users_ResponsableId",
                table: "Acciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Acciones_ResponsablesProceso_ResponsableId",
                table: "Acciones",
                column: "ResponsableId",
                principalTable: "ResponsablesProceso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Acciones_ResponsablesProceso_ResponsableId",
                table: "Acciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Acciones_Users_ResponsableId",
                table: "Acciones",
                column: "ResponsableId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
