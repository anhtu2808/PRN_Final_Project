using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRentalManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderLogsAndImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())"),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLog_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLogImgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderLogId = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLogImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLogImg_OrderLog",
                        column: x => x.OrderLogId,
                        principalTable: "OrderLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLogImgs_OrderLogId",
                table: "OrderLogImgs",
                column: "OrderLogId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLogs_OrderId",
                table: "OrderLogs",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLogImgs");

            migrationBuilder.DropTable(
                name: "OrderLogs");
        }
    }
}
