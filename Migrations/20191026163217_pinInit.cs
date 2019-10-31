using Microsoft.EntityFrameworkCore.Migrations;

namespace CutitBz.Migrations
{
    public partial class pinInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pin",
                table: "Links",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pin",
                table: "Links");
        }
    }
}
