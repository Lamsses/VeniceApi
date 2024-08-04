using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeniceApi.Migrations
{
    /// <inheritdoc />
    public partial class modifyorderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedDiscount",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrecentageDiscount",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedDiscount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PrecentageDiscount",
                table: "OrderItems");
        }
    }
}
