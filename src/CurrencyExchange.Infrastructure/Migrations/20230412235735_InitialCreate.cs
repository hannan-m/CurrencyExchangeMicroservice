using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CurrencyExchange.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    SourceCurrency = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCurrency = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeTransactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "client1@medirect.com", "Client 1" },
                    { 2, "client2@medirect.com", "Client 2" },
                    { 3, "client3@medirect.com", "Client 3" }
                });

            migrationBuilder.InsertData(
                table: "CurrencyExchangeTransactions",
                columns: new[] { "Id", "Amount", "ClientId", "Rate", "SourceCurrency", "TargetCurrency", "Timestamp" },
                values: new object[,]
                {
                    { 1, 100m, 1, 1.2m, 0, 1, new DateTime(2023, 4, 12, 22, 57, 35, 171, DateTimeKind.Utc).AddTicks(7236) },
                    { 2, 200m, 2, 0.8m, 1, 0, new DateTime(2023, 4, 12, 21, 57, 35, 171, DateTimeKind.Utc).AddTicks(7248) },
                    { 3, 150m, 3, 1.1m, 0, 2, new DateTime(2023, 4, 12, 20, 57, 35, 171, DateTimeKind.Utc).AddTicks(7250) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeTransactions_ClientId",
                table: "CurrencyExchangeTransactions",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyExchangeTransactions");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
