using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace npost.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:unaccent", ",,");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuarioid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    sobrenome = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: false),
                    senha = table.Column<string>(type: "character varying(64)", unicode: false, maxLength: 64, nullable: false),
                    email = table.Column<string>(type: "character varying(70)", unicode: false, maxLength: 70, nullable: false),
                    telefone = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    dtnascimento = table.Column<DateTime>(type: "date", nullable: false),
                    darkmode = table.Column<bool>(type: "boolean", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkUsuario", x => x.usuarioid);
                });

            migrationBuilder.CreateTable(
                name: "notations",
                columns: table => new
                {
                    notationid = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(70)", unicode: false, maxLength: 70, nullable: false),
                    content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkNotation", x => x.notationid);
                    table.ForeignKey(
                        name: "fkNotationUsuario",
                        column: x => x.userid,
                        principalTable: "usuarios",
                        principalColumn: "usuarioid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_notations_userid",
                table: "notations",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notations");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
