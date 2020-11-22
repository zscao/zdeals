using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class AddDealPriceSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriceSource",
                table: "DealPrices",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceSourceId",
                table: "DealPrices",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceSource",
                table: "DealPrices");

            migrationBuilder.DropColumn(
                name: "PriceSourceId",
                table: "DealPrices");
        }
    }
}
