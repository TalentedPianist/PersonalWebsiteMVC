using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class GuestbookChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Guestbook",
                newName: "GuestbookDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GuestbookDate",
                table: "Guestbook",
                newName: "DatePosted");
        }
    }
}
