using Microsoft.EntityFrameworkCore.Migrations;

namespace Sidekick.Database.Migrations
{
    public partial class v1000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sidekick");

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
                name: "Windows",
                schema: "sidekick");
        }
    }
}
