using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDeals.Data.Migrations
{
    public partial class DealActionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "VerifiedTime",
                table: "Deals");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Deals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DealId = table.Column<int>(nullable: false),
                    Action = table.Column<string>(maxLength: 20, nullable: false),
                    ActedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ActedOn = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionHistory_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deals_Status",
                table: "Deals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ActionHistory_DealId",
                table: "ActionHistory",
                column: "DealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionHistory");

            migrationBuilder.DropIndex(
                name: "IX_Deals_Status",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Deals");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Deals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "Deals",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Deals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "Deals",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedTime",
                table: "Deals",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
