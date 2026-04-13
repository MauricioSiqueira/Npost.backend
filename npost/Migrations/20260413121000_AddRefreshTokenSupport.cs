using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace npost.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "refreshtokenexpiresat",
                table: "usuarios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refreshtokenhash",
                table: "usuarios",
                type: "character varying(64)",
                unicode: false,
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refreshtokenexpiresat",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "refreshtokenhash",
                table: "usuarios");
        }
    }
}
