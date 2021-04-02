using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class PhotoMediumUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Photos",
                newName: "PhotoMediumUrl");

            migrationBuilder.AddColumn<string>(
                name: "PhotoLargeUrl",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoLargeUrl",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "PhotoMediumUrl",
                table: "Photos",
                newName: "PhotoUrl");
        }
    }
}
