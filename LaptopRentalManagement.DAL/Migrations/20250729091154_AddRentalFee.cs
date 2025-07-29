using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRentalManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RentalFee",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalFee",
                table: "Order");
        }
    }
}
