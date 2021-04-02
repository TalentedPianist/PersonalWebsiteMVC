using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class RemoteID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GalleryRemoteID",
                table: "Gallery",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GalleryRemoteID",
                table: "Gallery");
        }
    }
}
