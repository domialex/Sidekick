using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Persistence.Migrations
{
    public partial class Version_1205 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NinjaPrices",
                schema: "sidekick",
                table: "NinjaPrices");

            migrationBuilder.AddColumn<int>(
                name: "MapTier",
                schema: "sidekick",
                table: "NinjaPrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GemLevel",
                schema: "sidekick",
                table: "NinjaPrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NinjaPrices",
                schema: "sidekick",
                table: "NinjaPrices",
                columns: new[] { "Name", "Corrupted", "MapTier", "GemLevel" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NinjaPrices",
                schema: "sidekick",
                table: "NinjaPrices");

            migrationBuilder.DropColumn(
                name: "MapTier",
                schema: "sidekick",
                table: "NinjaPrices");

            migrationBuilder.DropColumn(
                name: "GemLevel",
                schema: "sidekick",
                table: "NinjaPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NinjaPrices",
                schema: "sidekick",
                table: "NinjaPrices",
                columns: new[] { "Name", "Corrupted" });
        }
    }
}
