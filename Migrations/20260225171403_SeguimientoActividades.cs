using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadApp.Migrations
{
    /// <inheritdoc />
    public partial class SeguimientoActividades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ejecutada",
                table: "Actividades",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEjecucion",
                table: "Actividades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreArchivoEvidencia",
                table: "Actividades",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Actividades",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RutaEvidencia",
                table: "Actividades",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ejecutada",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "FechaEjecucion",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "NombreArchivoEvidencia",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Actividades");

            migrationBuilder.DropColumn(
                name: "RutaEvidencia",
                table: "Actividades");
        }
    }
}
