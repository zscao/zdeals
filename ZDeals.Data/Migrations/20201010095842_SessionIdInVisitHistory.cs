using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class SessionIdInVisitHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "VisitHistory",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionToken",
                table: "VisitHistory",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "VisitHistory");

            migrationBuilder.DropColumn(
                name: "SessionToken",
                table: "VisitHistory");
        }
    }
}
