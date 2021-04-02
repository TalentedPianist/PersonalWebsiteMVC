using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalWebsiteMVC.Migrations
{
    public partial class GuestbookChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Guestbook");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
