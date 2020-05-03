using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class AddVerfiedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Deals");

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "Deals",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedTime",
                table: "Deals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "VerifiedTime",
                table: "Deals");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "Deals",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
