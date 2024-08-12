using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeniceApi.Migrations
{
    /// <inheritdoc />
    public partial class eaga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Expenses");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
