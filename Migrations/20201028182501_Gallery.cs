using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class Gallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryName = table.Column<string>(nullable: true),
                    GalleryDescription = table.Column<string>(nullable: true),
                    GalleryLocation = table.Column<string>(nullable: true),
                    GalleryDate = table.Column<DateTime>(nullable: false),
                    GalleryIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryID);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryID = table.Column<int>(nullable: false),
                    PhotoName = table.Column<string>(nullable: true),
                    PhotoDescription = table.Column<string>(nullable: true),
                    PhotoLocation = table.Column<string>(nullable: true),
                    PhotoDate = table.Column<DateTime>(nullable: false),
                    PhotoIP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
