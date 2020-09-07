using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class AddVerifiedToDeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Deals",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Deals");
        }
    }
}
