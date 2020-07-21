using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Engine.Data.Migrations
{
    public partial class ChangedCreatedTimeOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Products",
                newName: "UpdatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "Products",
                newName: "CreatedTime");
        }
    }
}
