using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class PhotoRemoteID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoRemoteID",
                table: "Photos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoRemoteID",
                table: "Photos");
        }
    }
}
