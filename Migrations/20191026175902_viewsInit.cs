using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CutitBz.Migrations
{
    public partial class viewsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "views",
                table: "Links",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Views",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ip = table.Column<string>(nullable: true),
                    link = table.Column<string>(nullable: true),
                    date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Views", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Views");

            migrationBuilder.DropColumn(
                name: "views",
                table: "Links");
        }
    }
}
