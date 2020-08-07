using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class DealVisitStatis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitedTime",
                table: "Deals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalVisited",
                table: "Deals",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastVisitedTime",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TotalVisited",
                table: "Deals");
        }
    }
}
