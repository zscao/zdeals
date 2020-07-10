using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Engine.Data.Migrations
{
    public partial class LengthOnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Store",
                table: "Products",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(60) CHARACTER SET utf8mb4",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sku",
                table: "Products",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30) CHARACTER SET utf8mb4",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mpn",
                table: "Products",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30) CHARACTER SET utf8mb4",
                oldMaxLength: 30,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Store",
                table: "Products",
                type: "varchar(60) CHARACTER SET utf8mb4",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sku",
                table: "Products",
                type: "varchar(30) CHARACTER SET utf8mb4",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mpn",
                table: "Products",
                type: "varchar(30) CHARACTER SET utf8mb4",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
