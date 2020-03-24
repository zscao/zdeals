using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class FixTypos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrition",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "HighLight",
                table: "Deals",
                newName: "Highlight");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Deals",
                maxLength: 2000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "Highlight",
                table: "Deals",
                newName: "HighLight");

            migrationBuilder.AddColumn<string>(
                name: "Descrition",
                table: "Deals",
                type: "varchar(2000) CHARACTER SET utf8mb4",
                maxLength: 2000,
                nullable: true);
        }
    }
}
