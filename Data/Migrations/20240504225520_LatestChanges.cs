using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class LatestChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestbookComment",
                table: "Guestbook");

            migrationBuilder.DropColumn(
                name: "GuestbookIP",
                table: "Guestbook");

            migrationBuilder.RenameColumn(
                name: "GuestbookUserWebsite",
                table: "Guestbook",
                newName: "GuestbookContent");

            migrationBuilder.RenameColumn(
                name: "GuestbookUserEmail",
                table: "Guestbook",
                newName: "GuestbookAuthorEmail");

            migrationBuilder.RenameColumn(
                name: "GuestbookUser",
                table: "Guestbook",
                newName: "GuestbookAuthor");

            migrationBuilder.RenameColumn(
                name: "GuestbookDate",
                table: "Guestbook",
                newName: "DatePosted");

            migrationBuilder.RenameColumn(
                name: "GuestbookID",
                table: "Guestbook",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PostTitle",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostLocation",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostIP",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostExcerpt",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostContent",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostAuthor",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PostActive",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GuestbookApproved",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "GuestbookAuthorIP",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestbookAuthorUrl",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestbookAuthorIP",
                table: "Guestbook");

            migrationBuilder.DropColumn(
                name: "GuestbookAuthorUrl",
                table: "Guestbook");

            migrationBuilder.RenameColumn(
                name: "GuestbookContent",
                table: "Guestbook",
                newName: "GuestbookUserWebsite");

            migrationBuilder.RenameColumn(
                name: "GuestbookAuthorEmail",
                table: "Guestbook",
                newName: "GuestbookUserEmail");

            migrationBuilder.RenameColumn(
                name: "GuestbookAuthor",
                table: "Guestbook",
                newName: "GuestbookUser");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Guestbook",
                newName: "GuestbookDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Guestbook",
                newName: "GuestbookID");

            migrationBuilder.AlterColumn<string>(
                name: "PostTitle",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostLocation",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostIP",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostExcerpt",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostContent",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostAuthor",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostActive",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuestbookApproved",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestbookComment",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestbookIP",
                table: "Guestbook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
