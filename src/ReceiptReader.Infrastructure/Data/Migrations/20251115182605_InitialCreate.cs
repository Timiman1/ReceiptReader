using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    FileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoreValue = table.Column<string>(type: "TEXT", nullable: true),
                    StoreConfidence = table.Column<double>(type: "REAL", nullable: false),
                    StoreSourceText = table.Column<string>(type: "TEXT", nullable: true),
                    AmountValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    AmountConfidence = table.Column<double>(type: "REAL", nullable: false),
                    AmountSourceText = table.Column<string>(type: "TEXT", nullable: true),
                    DateValue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateConfidence = table.Column<double>(type: "REAL", nullable: false),
                    DateSourceText = table.Column<string>(type: "TEXT", nullable: true),
                    RawText = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.FileId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receipts");
        }
    }
}
