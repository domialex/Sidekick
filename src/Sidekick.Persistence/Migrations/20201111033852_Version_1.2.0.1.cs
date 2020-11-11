using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Persistence.Migrations
{
    public partial class Version_1201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Windows",
                schema: "sidekick");

            migrationBuilder.CreateTable(
                name: "ViewPreferences",
                schema: "sidekick",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewPreferences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewPreferences",
                schema: "sidekick");

            migrationBuilder.CreateTable(
                name: "Windows",
                schema: "sidekick",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Windows", x => x.Id);
                });
        }
    }
}
