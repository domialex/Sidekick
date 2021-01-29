using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Persistence.Migrations
{
    public partial class Version_2000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sidekick");

            migrationBuilder.CreateTable(
                name: "Caches",
                schema: "sidekick",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caches", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                schema: "sidekick",
                columns: table => new
                {
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                schema: "sidekick",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NinjaPrices",
                schema: "sidekick",
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
                    table.PrimaryKey("PK_NinjaPrices", x => new { x.Name, x.Corrupted, x.MapTier, x.GemLevel });
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

            migrationBuilder.CreateTable(
                name: "ViewPreferences",
                schema: "sidekick",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewPreferences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caches",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "ItemCategories",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "Leagues",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "NinjaPrices",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "NinjaTranslations",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "ViewPreferences",
                schema: "sidekick");
        }
    }
}
