using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Persistence.Migrations
{
    public partial class Version_1203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NinjaPrices",
                schema: "sidekick",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Corrupted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaPrices", x => new { x.Name, x.Corrupted });
                });

            migrationBuilder.CreateTable(
                name: "NinjaTranslations",
                schema: "sidekick",
                columns: table => new
                {
                    Translation = table.Column<string>(type: "TEXT", nullable: false),
                    English = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaTranslations", x => x.Translation);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NinjaPrices",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "NinjaTranslations",
                schema: "sidekick");
        }
    }
}
