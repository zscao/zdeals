using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Engine.Data.Migrations
{
    public partial class SourceOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Products",
                nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "ParsedTime",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Store",
                table: "Products",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Products",
                maxLength: 400,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ParsedTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Store",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Products");
        }
    }
}
