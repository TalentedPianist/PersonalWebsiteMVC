using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class PCloudFolderID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PCloudFolderID",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PCloudFolderID",
                table: "Albums");
        }
    }
}
