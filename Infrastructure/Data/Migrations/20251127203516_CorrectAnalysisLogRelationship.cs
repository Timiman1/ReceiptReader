using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Migrations
{
    /// <inheritdoc />
    public partial class CorrectAnalysisLogRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisLog_Receipts_ReceiptInfoId",
                table: "AnalysisLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalysisLog",
                table: "AnalysisLog");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisLog_ReceiptInfoId",
                table: "AnalysisLog");

            migrationBuilder.RenameTable(
                name: "AnalysisLog",
                newName: "AnalysisLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalysisLogs",
                table: "AnalysisLogs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLogs_ReceiptInfoId",
                table: "AnalysisLogs",
                column: "ReceiptInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisLogs_Receipts_ReceiptInfoId",
                table: "AnalysisLogs",
                column: "ReceiptInfoId",
                principalTable: "Receipts",
                principalColumn: "FileId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisLogs_Receipts_ReceiptInfoId",
                table: "AnalysisLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalysisLogs",
                table: "AnalysisLogs");

            migrationBuilder.DropIndex(
                name: "IX_AnalysisLogs_ReceiptInfoId",
                table: "AnalysisLogs");

            migrationBuilder.RenameTable(
                name: "AnalysisLogs",
                newName: "AnalysisLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalysisLog",
                table: "AnalysisLog",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLog_ReceiptInfoId",
                table: "AnalysisLog",
                column: "ReceiptInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisLog_Receipts_ReceiptInfoId",
                table: "AnalysisLog",
                column: "ReceiptInfoId",
                principalTable: "Receipts",
                principalColumn: "FileId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
