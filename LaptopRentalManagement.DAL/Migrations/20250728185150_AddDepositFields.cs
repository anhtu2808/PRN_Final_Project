using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRentalManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DepositAmount",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Laptop",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositAmount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Laptop");
        }
    }
}
