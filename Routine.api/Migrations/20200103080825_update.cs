using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.api.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introductionn",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Introductionn",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
