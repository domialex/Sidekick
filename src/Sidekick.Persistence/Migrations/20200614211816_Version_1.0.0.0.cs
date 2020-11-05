using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Persistence.Migrations
{
    public partial class Version_1000 : Migration
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
                    Key = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caches", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Windows",
                schema: "sidekick",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    Width = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Windows", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caches",
                schema: "sidekick");

            migrationBuilder.DropTable(
                name: "Windows",
                schema: "sidekick");
        }
    }
}
