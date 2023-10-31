using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simbir.GO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRevokedToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expiry_date",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "is_used",
                table: "refresh_tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expiry_date",
                table: "refresh_tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_used",
                table: "refresh_tokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
