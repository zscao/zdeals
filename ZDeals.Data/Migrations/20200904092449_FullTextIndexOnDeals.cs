using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class FullTextIndexOnDeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deals_Title",
                table: "Deals");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Deals",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(400) CHARACTER SET utf8mb4",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Title",
                table: "Deals",
                column: "Title")
                .Annotation("MySql:FullTextIndex", true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Code",
                table: "Brands",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deals_Title",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Code",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Deals",
                type: "varchar(400) CHARACTER SET utf8mb4",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400);

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Title",
                table: "Deals",
                column: "Title");
        }
    }
}
