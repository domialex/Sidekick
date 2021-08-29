using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Apis.PoeNinja.Migrations
{
    public partial class PoeNinjaRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Corrupted = table.Column<bool>(type: "INTEGER", nullable: false),
                    MapTier = table.Column<int>(type: "INTEGER", nullable: false),
                    GemLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => new { x.Name, x.Corrupted, x.MapTier, x.GemLevel });
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Translation = table.Column<string>(type: "TEXT", nullable: false),
                    English = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Translation);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
