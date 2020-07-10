using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Engine.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    HighLight = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    FullPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PriceCurrency = table.Column<string>(maxLength: 3, nullable: false),
                    Manufacturer = table.Column<string>(maxLength: 50, nullable: true),
                    Brand = table.Column<string>(maxLength: 20, nullable: true),
                    Sku = table.Column<string>(maxLength: 30, nullable: true),
                    Mpn = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
