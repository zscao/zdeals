using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class RemoveDealPictureEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DealPictures",
                table: "DealPictures");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DealPictures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealPictures",
                table: "DealPictures",
                columns: new[] { "FileName", "DealId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DealPictures",
                table: "DealPictures");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DealPictures",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealPictures",
                table: "DealPictures",
                column: "Id");
        }
    }
}
