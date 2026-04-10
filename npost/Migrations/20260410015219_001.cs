using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace npost.Migrations
{
    /// <inheritdoc />
    public partial class _001 : Migration
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
                    dtnascimento = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false),
                    darkmode = table.Column<bool>(type: "boolean", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkUsuario", x => x.usuarioid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
