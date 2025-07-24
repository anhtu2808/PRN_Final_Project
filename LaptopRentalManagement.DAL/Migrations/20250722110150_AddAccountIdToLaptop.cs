using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRentalManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Laptop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_Laptop_AccountId",
                table: "Laptop",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "fk_Laptop_Account",
                table: "Laptop",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_Laptop_Account",
                table: "Laptop");

            migrationBuilder.DropIndex(
                name: "ix_Laptop_AccountId",
                table: "Laptop");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Laptop");
        }
    }
}
