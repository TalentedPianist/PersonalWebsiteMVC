using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class GalleryRemoteID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "GalleryRemoteID",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          }
    }
}
