using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Rents.Enums;
using Simbir.GO.Domain.Transports.Enums;

#nullable disable

namespace Simbir.GO.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    balance_Value = table.Column<double>(type: "double precision", nullable: false),
                    role = table.Column<Role>(type: "role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    jwtId = table.Column<string>(type: "text", nullable: false),
                    isUsed = table.Column<bool>(type: "boolean", nullable: false),
                    isRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    addedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ownerId = table.Column<long>(type: "bigint", nullable: false),
                    canBeRented = table.Column<bool>(type: "boolean", nullable: false),
                    transportType = table.Column<TransportType>(type: "transport_type", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    identifier = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    minutePrice = table.Column<double>(type: "double precision", nullable: true),
                    dayPrice = table.Column<double>(type: "double precision", nullable: true),
                    coordinate_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    coordinate_Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_transports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transportId = table.Column<long>(type: "bigint", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    timeStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    timeEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    priceOfUnit = table.Column<double>(type: "double precision", nullable: false),
                    priceType = table.Column<PriceType>(type: "price_type", nullable: false),
                    finalPrice = table.Column<double>(type: "double precision", nullable: true),
                    accountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_rents", x => x.id);
                    table.ForeignKey(
                        name: "fK_rents_accounts_AccountTempId",
                        column: x => x.accountId,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_rents_transports_transportTempId",
                        column: x => x.transportId,
                        principalTable: "transports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_rents_accountId",
                table: "rents",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "iX_rents_transportId",
                table: "rents",
                column: "transportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "rents");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "transports");
        }
    }
}
