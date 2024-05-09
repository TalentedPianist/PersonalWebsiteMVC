using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Albums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryRemoteID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GalleryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GalleryIP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Gallery");
        }
    }
}
