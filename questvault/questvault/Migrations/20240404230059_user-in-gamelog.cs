using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class useringamelog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GameLog",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLog_UserId",
                table: "GameLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLog_AspNetUsers_UserId",
                table: "GameLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLog_AspNetUsers_UserId",
                table: "GameLog");

            migrationBuilder.DropIndex(
                name: "IX_GameLog_UserId",
                table: "GameLog");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameLog");
        }
    }
}
