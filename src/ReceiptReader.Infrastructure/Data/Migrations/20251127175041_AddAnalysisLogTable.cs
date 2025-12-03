using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceiptReader.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalysisLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileHash = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    AnalysisDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    FailureReason = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiptInfoId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisLog_Receipts_ReceiptInfoId",
                        column: x => x.ReceiptInfoId,
                        principalTable: "Receipts",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisLog_ReceiptInfoId",
                table: "AnalysisLog",
                column: "ReceiptInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisLog");
        }
    }
}
