using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Photos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GalleryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryRemoteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryID);
                });
        }
    }
}
