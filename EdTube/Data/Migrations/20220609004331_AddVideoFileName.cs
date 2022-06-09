using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdTube.Data.Migrations
{
    public partial class AddVideoFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelId",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ChannelId",
                table: "Videos",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Channels_ChannelId",
                table: "Videos",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Channels_ChannelId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_ChannelId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Videos");
        }
    }
}
