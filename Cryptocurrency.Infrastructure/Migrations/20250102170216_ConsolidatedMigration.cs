using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptocurrency.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ConsolidatedMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CryptoSymbols",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Symbol = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                UserId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CryptoSymbols", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                UserId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserName = table.Column<string>(type: "TEXT", nullable: false),
                Password = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.UserId);
            });

        migrationBuilder.CreateTable(
            name: "ExchangeRates",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Currency = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                Price = table.Column<decimal>(type: "TEXT", nullable: false),
                Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                CryptoSymbolId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExchangeRates_CryptoSymbols_CryptoSymbolId",
                    column: x => x.CryptoSymbolId,
                    principalTable: "CryptoSymbols",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SearchHistories",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<int>(type: "INTEGER", nullable: false),
                CryptoSymbolId = table.Column<int>(type: "INTEGER", nullable: false),
                SearchedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SearchHistories", x => x.Id);
                table.ForeignKey(
                    name: "FK_SearchHistories_CryptoSymbols_CryptoSymbolId",
                    column: x => x.CryptoSymbolId,
                    principalTable: "CryptoSymbols",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ExchangeRates_CryptoSymbolId",
            table: "ExchangeRates",
            column: "CryptoSymbolId");

        migrationBuilder.CreateIndex(
            name: "IX_SearchHistories_CryptoSymbolId",
            table: "SearchHistories",
            column: "CryptoSymbolId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ExchangeRates");

        migrationBuilder.DropTable(
            name: "SearchHistories");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "CryptoSymbols");
    }
}
