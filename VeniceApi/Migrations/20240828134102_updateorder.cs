using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeniceApi.Migrations
{
    /// <inheritdoc />
    public partial class updateorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint if exists (for OrderItems)
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            // Drop the primary key for Orders
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            // Drop the primary key for OrderItems
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            // Drop the Id column in Orders
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            // Drop the OrderId column in OrderItems
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItems");

            // Add the new Id column in Orders
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"); // Generate new GUID as default

            // Add the new OrderId column in OrderItems
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false);

            // Re-add the primary key for Orders
            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            // Re-add the primary key for OrderItems
            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "OrderId");

            // Re-add the foreign key constraint for OrderItems
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraint if exists (for OrderItems)
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            // Drop the primary key for Orders
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            // Drop the primary key for OrderItems
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            // Drop the Id column in Orders
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            // Drop the OrderId column in OrderItems
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItems");

            // Recreate the old Id column in Orders
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Recreate the old OrderId column in OrderItems
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Re-add the primary key for Orders
            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            // Re-add the primary key for OrderItems
            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "OrderId");

            // Re-add the foreign key constraint for OrderItems
            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
