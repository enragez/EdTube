using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdTube.Data.Migrations
{
    public partial class AddVideoUserRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BecomeAuthorRequests",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BecomeAuthorRequests_UserId",
                table: "BecomeAuthorRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BecomeAuthorRequests_AspNetUsers_UserId",
                table: "BecomeAuthorRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BecomeAuthorRequests_AspNetUsers_UserId",
                table: "BecomeAuthorRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_UserId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_BecomeAuthorRequests_UserId",
                table: "BecomeAuthorRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Videos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BecomeAuthorRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
