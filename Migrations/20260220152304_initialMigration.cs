using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActividadApp.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Maestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maestros", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResponsablesProceso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsablesProceso", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Procesos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponsableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procesos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procesos_ResponsablesProceso_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "ResponsablesProceso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrganizacionId = table.Column<int>(type: "int", nullable: false),
                    Cedula = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CargoId = table.Column<int>(type: "int", nullable: false),
                    AgenciaId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Maestros_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Maestros_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Maestros_OrganizacionId",
                        column: x => x.OrganizacionId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Acciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TipoAccionId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcesoId = table.Column<int>(type: "int", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    OrigenId = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    SistemaGestionId = table.Column<int>(type: "int", nullable: false),
                    OrganizacionId = table.Column<int>(type: "int", nullable: false),
                    SitioId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Acciones_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Maestros_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Maestros_OrganizacionId",
                        column: x => x.OrganizacionId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Maestros_OrigenId",
                        column: x => x.OrigenId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Maestros_SistemaGestionId",
                        column: x => x.SistemaGestionId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Maestros_SitioId",
                        column: x => x.SitioId,
                        principalTable: "Maestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Procesos_ProcesoId",
                        column: x => x.ProcesoId,
                        principalTable: "Procesos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Users_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Acciones_Users_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConfirmacionesPlanAccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccionId = table.Column<int>(type: "int", nullable: false),
                    DetallesCumplimiento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    CumplioPlan = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmacionesPlanAccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmacionesPlanAccion_Acciones_AccionId",
                        column: x => x.AccionId,
                        principalTable: "Acciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfirmacionesPlanAccion_Users_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Eficacias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccionId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Eficaz = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eficacias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eficacias_Acciones_AccionId",
                        column: x => x.AccionId,
                        principalTable: "Acciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Soluciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccionId = table.Column<int>(type: "int", nullable: false),
                    CorreccionPropuesta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AnalisisCausa = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioInvestigaId = table.Column<int>(type: "int", nullable: false),
                    FechaInvestigacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioCoordinaId = table.Column<int>(type: "int", nullable: false),
                    FechaCompromiso = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soluciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Soluciones_Acciones_AccionId",
                        column: x => x.AccionId,
                        principalTable: "Acciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Soluciones_Users_UsuarioCoordinaId",
                        column: x => x.UsuarioCoordinaId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Soluciones_Users_UsuarioInvestigaId",
                        column: x => x.UsuarioInvestigaId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SolucionId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    Agencia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fecha = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actividades_Soluciones_SolucionId",
                        column: x => x.SolucionId,
                        principalTable: "Soluciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Actividades_Users_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_EntidadId",
                table: "Acciones",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_EstadoId",
                table: "Acciones",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_Id",
                table: "Acciones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_OrganizacionId",
                table: "Acciones",
                column: "OrganizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_OrigenId",
                table: "Acciones",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_ProcesoId",
                table: "Acciones",
                column: "ProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_ResponsableId",
                table: "Acciones",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_SistemaGestionId",
                table: "Acciones",
                column: "SistemaGestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_SitioId",
                table: "Acciones",
                column: "SitioId");

            migrationBuilder.CreateIndex(
                name: "IX_Acciones_UsuarioId",
                table: "Acciones",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_ResponsableId",
                table: "Actividades",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_SolucionId",
                table: "Actividades",
                column: "SolucionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmacionesPlanAccion_AccionId",
                table: "ConfirmacionesPlanAccion",
                column: "AccionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmacionesPlanAccion_ResponsableId",
                table: "ConfirmacionesPlanAccion",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Eficacias_AccionId",
                table: "Eficacias",
                column: "AccionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Procesos_ResponsableId",
                table: "Procesos",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Soluciones_AccionId",
                table: "Soluciones",
                column: "AccionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Soluciones_UsuarioCoordinaId",
                table: "Soluciones",
                column: "UsuarioCoordinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Soluciones_UsuarioInvestigaId",
                table: "Soluciones",
                column: "UsuarioInvestigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AgenciaId",
                table: "Users",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CargoId",
                table: "Users",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizacionId",
                table: "Users",
                column: "OrganizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId",
                table: "Users",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actividades");

            migrationBuilder.DropTable(
                name: "ConfirmacionesPlanAccion");

            migrationBuilder.DropTable(
                name: "Eficacias");

            migrationBuilder.DropTable(
                name: "Soluciones");

            migrationBuilder.DropTable(
                name: "Acciones");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Procesos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ResponsablesProceso");

            migrationBuilder.DropTable(
                name: "Maestros");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
