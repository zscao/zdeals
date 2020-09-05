using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class RemoveVisitedFromDeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastVisitedTime",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TotalVisited",
                table: "Deals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Deals",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Deals",
                nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Deals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                table: "Deals",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitedTime",
                table: "Deals",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalVisited",
                table: "Deals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
