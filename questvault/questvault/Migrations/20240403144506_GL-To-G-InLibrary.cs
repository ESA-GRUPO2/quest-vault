using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class GLToGInLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLog_GamesLibrary_GamesLibraryId1",
                table: "GameLog");

            migrationBuilder.DropIndex(
                name: "IX_GameLog_GamesLibraryId1",
                table: "GameLog");

            migrationBuilder.DropColumn(
                name: "GamesLibraryId1",
                table: "GameLog");

            migrationBuilder.AddColumn<long>(
                name: "GamesLibraryId",
                table: "Games",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GamesLibraryId",
                table: "Games",
                column: "GamesLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GamesLibrary_GamesLibraryId",
                table: "Games",
                column: "GamesLibraryId",
                principalTable: "GamesLibrary",
                principalColumn: "GamesLibraryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GamesLibrary_GamesLibraryId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GamesLibraryId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GamesLibraryId",
                table: "Games");

            migrationBuilder.AddColumn<long>(
                name: "GamesLibraryId1",
                table: "GameLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLog_GamesLibraryId1",
                table: "GameLog",
                column: "GamesLibraryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLog_GamesLibrary_GamesLibraryId1",
                table: "GameLog",
                column: "GamesLibraryId1",
                principalTable: "GamesLibrary",
                principalColumn: "GamesLibraryId");
        }
    }
}
