using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadApp.Migrations
{
    /// <inheritdoc />
    public partial class SeguimientoSgiYDiasEficacia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiasParaVerificar",
                table: "Eficacias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaNotificacion",
                table: "Acciones",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeguimientosSgi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeguimientosSgi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeguimientosSgi_Acciones_AccionId",
                        column: x => x.AccionId,
                        principalTable: "Acciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeguimientosSgi_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosSgi_AccionId",
                table: "SeguimientosSgi",
                column: "AccionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeguimientosSgi_UsuarioId",
                table: "SeguimientosSgi",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeguimientosSgi");

            migrationBuilder.DropColumn(
                name: "DiasParaVerificar",
                table: "Eficacias");

            migrationBuilder.DropColumn(
                name: "UltimaNotificacion",
                table: "Acciones");
        }
    }
}
