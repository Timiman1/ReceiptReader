using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReceiptDetailTablesAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxableAmount",
                table: "ReceiptTaxLines",
                newName: "NetAmount");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiptInfoId",
                table: "ReceiptTaxLines",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "ReceiptTaxLines",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiptInfoId",
                table: "ReceiptLineItem",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "QuantityType",
                table: "ReceiptLineItem",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "ReceiptTaxLines");

            migrationBuilder.DropColumn(
                name: "QuantityType",
                table: "ReceiptLineItem");

            migrationBuilder.RenameColumn(
                name: "NetAmount",
                table: "ReceiptTaxLines",
                newName: "TaxableAmount");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiptInfoId",
                table: "ReceiptTaxLines",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiptInfoId",
                table: "ReceiptLineItem",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
