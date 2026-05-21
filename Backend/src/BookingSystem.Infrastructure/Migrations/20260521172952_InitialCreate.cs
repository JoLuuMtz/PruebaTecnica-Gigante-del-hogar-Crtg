using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    cod = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.cod);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    cod = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    clave = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    razon_social = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.cod);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prestadores",
                columns: table => new
                {
                    cod_usuario = table.Column<int>(type: "int", nullable: false),
                    especialidad = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prestadores", x => x.cod_usuario);
                    table.ForeignKey(
                        name: "FK_prestadores_usuarios_cod_usuario",
                        column: x => x.cod_usuario,
                        principalTable: "usuarios",
                        principalColumn: "cod",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "solicitantes",
                columns: table => new
                {
                    cod_usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitantes", x => x.cod_usuario);
                    table.ForeignKey(
                        name: "FK_solicitantes_usuarios_cod_usuario",
                        column: x => x.cod_usuario,
                        principalTable: "usuarios",
                        principalColumn: "cod",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios_roles",
                columns: table => new
                {
                    cod_usuario = table.Column<int>(type: "int", nullable: false),
                    cod_rol = table.Column<int>(type: "int", nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_roles", x => new { x.cod_usuario, x.cod_rol });
                    table.ForeignKey(
                        name: "FK_usuarios_roles_roles_cod_rol",
                        column: x => x.cod_rol,
                        principalTable: "roles",
                        principalColumn: "cod",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_usuarios_cod_usuario",
                        column: x => x.cod_usuario,
                        principalTable: "usuarios",
                        principalColumn: "cod",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    cod = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    cupos_totales = table.Column<int>(type: "int", nullable: false),
                    cupos_disponibles = table.Column<int>(type: "int", nullable: false),
                    cod_usuario_prestador = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citas", x => x.cod);
                    table.ForeignKey(
                        name: "FK_citas_prestadores_cod_usuario_prestador",
                        column: x => x.cod_usuario_prestador,
                        principalTable: "prestadores",
                        principalColumn: "cod_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "solicitantes_prestadores",
                columns: table => new
                {
                    cod_usuario_solicitante = table.Column<int>(type: "int", nullable: false),
                    cod_usuario_prestador = table.Column<int>(type: "int", nullable: false),
                    fecha_suscripcion = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitantes_prestadores", x => new { x.cod_usuario_solicitante, x.cod_usuario_prestador });
                    table.ForeignKey(
                        name: "FK_solicitantes_prestadores_prestadores_cod_usuario_prestador",
                        column: x => x.cod_usuario_prestador,
                        principalTable: "prestadores",
                        principalColumn: "cod_usuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitantes_prestadores_solicitantes_cod_usuario_solicitante",
                        column: x => x.cod_usuario_solicitante,
                        principalTable: "solicitantes",
                        principalColumn: "cod_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cupos",
                columns: table => new
                {
                    cod_cita = table.Column<int>(type: "int", nullable: false),
                    cod_usuario_solicitante = table.Column<int>(type: "int", nullable: false),
                    fecha_reserva = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cupos", x => new { x.cod_cita, x.cod_usuario_solicitante });
                    table.ForeignKey(
                        name: "FK_cupos_citas_cod_cita",
                        column: x => x.cod_cita,
                        principalTable: "citas",
                        principalColumn: "cod",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cupos_solicitantes_cod_usuario_solicitante",
                        column: x => x.cod_usuario_solicitante,
                        principalTable: "solicitantes",
                        principalColumn: "cod_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_citas_cod_usuario_prestador",
                table: "citas",
                column: "cod_usuario_prestador");

            migrationBuilder.CreateIndex(
                name: "IX_cupos_cod_usuario_solicitante",
                table: "cupos",
                column: "cod_usuario_solicitante");

            migrationBuilder.CreateIndex(
                name: "IX_solicitantes_prestadores_cod_usuario_prestador",
                table: "solicitantes_prestadores",
                column: "cod_usuario_prestador");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_usuario",
                table: "usuarios",
                column: "usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roles_cod_rol",
                table: "usuarios_roles",
                column: "cod_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cupos");

            migrationBuilder.DropTable(
                name: "solicitantes_prestadores");

            migrationBuilder.DropTable(
                name: "usuarios_roles");

            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "solicitantes");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "prestadores");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
