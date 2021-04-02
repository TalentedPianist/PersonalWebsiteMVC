using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class PostExcerpt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostExcerpt",
                table: "Posts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GalleryDate",
                table: "Gallery",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostExcerpt",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GalleryDate",
                table: "Gallery",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
