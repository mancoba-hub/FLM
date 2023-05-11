using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FLM.LisoMbiza.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    OpenDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    WeightedItem = table.Column<bool>(type: "bit", nullable: false),
                    SuggestedSellingPrice = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
