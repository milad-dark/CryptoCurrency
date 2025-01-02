using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptocurrency.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateExchangeRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CryptoSymbols_Symbol",
                table: "CryptoSymbols");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ExchangeRates",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "ExchangeRates",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExchangeRates",
                table: "ExchangeRates",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExchangeRates",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "ExchangeRates");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ExchangeRates",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_CryptoSymbols_Symbol",
                table: "CryptoSymbols",
                column: "Symbol",
                unique: true);
        }
    }
}
