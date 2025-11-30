using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptDetailTablesAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyConfidence",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CurrencySourceText",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "CurrencyValue",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TaxAmountConfidence",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TaxAmountSourceText",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TaxAmountValue",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TotalAmountConfidence",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TotalAmountSourceText",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TotalAmountValue",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TransactionDateConfidence",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TransactionDateSourceText",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TransactionDateValue",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "VendorNameConfidence",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "VendorNameSourceText",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "VendorNameValue",
                table: "Receipts",
                newName: "TransactionDate");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Receipts",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Receipts",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Receipts",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "Receipts",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ReceiptLineItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalLineAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProductCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ReceiptInfoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptLineItem_Receipts_ReceiptInfoId",
                        column: x => x.ReceiptInfoId,
                        principalTable: "Receipts",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptTaxLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxableAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Percentage = table.Column<decimal>(type: "TEXT", precision: 18, scale: 4, nullable: false),
                    ReceiptInfoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptTaxLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptTaxLines_Receipts_ReceiptInfoId",
                        column: x => x.ReceiptInfoId,
                        principalTable: "Receipts",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptLineItem_ReceiptInfoId",
                table: "ReceiptLineItem",
                column: "ReceiptInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptTaxLines_ReceiptInfoId",
                table: "ReceiptTaxLines",
                column: "ReceiptInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptLineItem");

            migrationBuilder.DropTable(
                name: "ReceiptTaxLines");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Receipts",
                newName: "VendorNameValue");

            migrationBuilder.AddColumn<double>(
                name: "CurrencyConfidence",
                table: "Receipts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySourceText",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyValue",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TaxAmountConfidence",
                table: "Receipts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TaxAmountSourceText",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmountValue",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmountConfidence",
                table: "Receipts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TotalAmountSourceText",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountValue",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TransactionDateConfidence",
                table: "Receipts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionDateSourceText",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDateValue",
                table: "Receipts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VendorNameConfidence",
                table: "Receipts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "VendorNameSourceText",
                table: "Receipts",
                type: "TEXT",
                nullable: true);
        }
    }
}
