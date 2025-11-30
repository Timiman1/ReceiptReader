using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiptSchemaV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoreValue",
                table: "Receipts",
                newName: "VendorNameValue");

            migrationBuilder.RenameColumn(
                name: "StoreSourceText",
                table: "Receipts",
                newName: "VendorNameSourceText");

            migrationBuilder.RenameColumn(
                name: "StoreConfidence",
                table: "Receipts",
                newName: "VendorNameConfidence");

            migrationBuilder.RenameColumn(
                name: "DateValue",
                table: "Receipts",
                newName: "TransactionDateValue");

            migrationBuilder.RenameColumn(
                name: "DateSourceText",
                table: "Receipts",
                newName: "TransactionDateSourceText");

            migrationBuilder.RenameColumn(
                name: "DateConfidence",
                table: "Receipts",
                newName: "TransactionDateConfidence");

            migrationBuilder.RenameColumn(
                name: "AmountValue",
                table: "Receipts",
                newName: "TotalAmountValue");

            migrationBuilder.RenameColumn(
                name: "AmountSourceText",
                table: "Receipts",
                newName: "TotalAmountSourceText");

            migrationBuilder.RenameColumn(
                name: "AmountConfidence",
                table: "Receipts",
                newName: "TotalAmountConfidence");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "VendorNameValue",
                table: "Receipts",
                newName: "StoreValue");

            migrationBuilder.RenameColumn(
                name: "VendorNameSourceText",
                table: "Receipts",
                newName: "StoreSourceText");

            migrationBuilder.RenameColumn(
                name: "VendorNameConfidence",
                table: "Receipts",
                newName: "StoreConfidence");

            migrationBuilder.RenameColumn(
                name: "TransactionDateValue",
                table: "Receipts",
                newName: "DateValue");

            migrationBuilder.RenameColumn(
                name: "TransactionDateSourceText",
                table: "Receipts",
                newName: "DateSourceText");

            migrationBuilder.RenameColumn(
                name: "TransactionDateConfidence",
                table: "Receipts",
                newName: "DateConfidence");

            migrationBuilder.RenameColumn(
                name: "TotalAmountValue",
                table: "Receipts",
                newName: "AmountValue");

            migrationBuilder.RenameColumn(
                name: "TotalAmountSourceText",
                table: "Receipts",
                newName: "AmountSourceText");

            migrationBuilder.RenameColumn(
                name: "TotalAmountConfidence",
                table: "Receipts",
                newName: "AmountConfidence");
        }
    }
}
