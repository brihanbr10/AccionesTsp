using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadApp.Migrations
{
    /// <inheritdoc />
    public partial class FixDiasParaVerificarColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE `SeguimientosSgi` 
                  ADD COLUMN `DiasParaVerificar` INT NOT NULL DEFAULT 90;",
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE `SeguimientosSgi` 
                  DROP COLUMN `DiasParaVerificar`;",
                suppressTransaction: true);
        }
    }
}
