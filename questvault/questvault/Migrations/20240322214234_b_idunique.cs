using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class b_idunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 20L,
                column: "IgdbRating",
                value: 86.734689216752713);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 71L,
                column: "IgdbRating",
                value: 86.778606969433241);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 72L,
                column: "IgdbRating",
                value: 91.849080624078795);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 233L,
                column: "IgdbRating",
                value: 90.65844625699026);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 472L,
                column: "IgdbRating",
                value: 87.692356473666834);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 732L,
                column: "IgdbRating",
                value: 90.588774009660852);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1009L,
                column: "IgdbRating",
                value: 93.376525089101833);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1020L,
                column: "IgdbRating",
                value: 89.82090131285085);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1942L,
                column: "IgdbRating",
                value: 94.145552782745355);

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_IgdbPlatformId",
                table: "Platforms",
                column: "IgdbPlatformId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_IgdbId",
                table: "Games",
                column: "IgdbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IgdbCompanyId",
                table: "Companies",
                column: "IgdbCompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Platforms_IgdbPlatformId",
                table: "Platforms");

            migrationBuilder.DropIndex(
                name: "IX_Games_IgdbId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Companies_IgdbCompanyId",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 20L,
                column: "IgdbRating",
                value: 86.734274201256369);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 71L,
                column: "IgdbRating",
                value: 86.778367407367497);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 72L,
                column: "IgdbRating",
                value: 91.84852424624583);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 233L,
                column: "IgdbRating",
                value: 90.658945635542821);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 472L,
                column: "IgdbRating",
                value: 87.692329442678826);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 732L,
                column: "IgdbRating",
                value: 90.588691922163221);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1009L,
                column: "IgdbRating",
                value: 93.376445042009308);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1020L,
                column: "IgdbRating",
                value: 89.82097825083261);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1942L,
                column: "IgdbRating",
                value: 94.145665829461763);
        }
    }
}
