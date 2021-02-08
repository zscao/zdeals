using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class RemovePriceSourceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceSourceId",
                table: "DealPrices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriceSourceId",
                table: "DealPrices",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true);
        }
    }
}
