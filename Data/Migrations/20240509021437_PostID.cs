using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "Comments",
                newName: "PostID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostID",
                table: "Comments",
                newName: "ParentID");
        }
    }
}
