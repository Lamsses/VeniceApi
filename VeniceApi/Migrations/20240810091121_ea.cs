using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeniceApi.Migrations
{
    /// <inheritdoc />
    public partial class ea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RandomId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomId",
                table: "Employees");
        }
    }
}
