using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class DeleteDeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Deals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "Deals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Title",
                table: "Deals",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deals_Title",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Deals");
        }
    }
}
