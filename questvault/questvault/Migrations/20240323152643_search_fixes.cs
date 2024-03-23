using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class search_fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 8L, 20L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 13L, 20L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 1L, 71L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 56L, 71L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 1L, 72L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 56L, 72L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 38L, 127L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 104L, 127L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 24L, 233L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 56L, 233L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 126L, 472L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 29L, 732L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 29L, 1020L });

            migrationBuilder.DeleteData(
                table: "GameCompany",
                keyColumns: new[] { "IgdbCompanyId", "IgdbId" },
                keyValues: new object[] { 50L, 1942L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 20L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 9L, 20L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 12L, 20L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 20L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 71L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 8L, 71L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 9L, 71L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 72L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 8L, 72L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 9L, 72L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 72L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 8L, 127L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 127L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 233L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 12L, 472L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 472L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 732L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 10L, 732L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 732L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 1009L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 1009L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 5L, 1020L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 10L, 1020L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 1020L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 12L, 1942L });

            migrationBuilder.DeleteData(
                table: "GameGenre",
                keyColumns: new[] { "IgdbGenreId", "IgdbId" },
                keyValues: new object[] { 31L, 1942L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 20L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 20L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 71L, 3L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 71L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 71L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 71L, 130L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 72L, 3L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 72L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 72L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 72L, 130L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 127L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 127L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 127L, 39L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 233L, 3L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 233L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 233L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 233L, 11L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 472L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 472L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 732L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 732L, 8L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 732L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 732L, 11L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 732L, 39L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1009L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1020L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1020L, 9L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1020L, 49L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1942L, 6L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1942L, 49L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1942L, 130L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1942L, 167L });

            migrationBuilder.DeleteData(
                table: "GamePlatform",
                keyColumns: new[] { "IgdbId", "IgdbPlatformId" },
                keyValues: new object[] { 1942L, 169L });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 126L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 127L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 233L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 472L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 732L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1009L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1020L);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1942L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 130L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 167L);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "PlatformId",
                keyValue: 169L);

            migrationBuilder.AddColumn<int>(
                name: "TotalRatingCount",
                table: "Games",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRatingCount",
                table: "Games");

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyName", "IgdbCompanyId" },
                values: new object[,]
                {
                    { 1L, "Electronic Arts", 1L },
                    { 8L, "2K Games", 8L },
                    { 13L, "Demiurge Studios", 13L },
                    { 24L, "Sierra Entertainment", 24L },
                    { 29L, "Rockstar Games", 29L },
                    { 38L, "Ubisoft Montreal", 38L },
                    { 50L, "WB Games", 50L },
                    { 56L, "Valve", 56L },
                    { 104L, "Ubisoft Entertainment", 104L },
                    { 126L, "Bethesda Game Studios", 126L }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "IgdbId", "IgdbRating", "ImageUrl", "IsReleased", "Name", "QvRating", "ReleaseDate", "Screenshots", "Summary", "VideoUrl" },
                values: new object[,]
                {
                    { 20L, 20L, 86.734689216752713, "//images.igdb.com/igdb/image/upload/t_cover_big/co2mli.jpg", true, "BioShock", 0, new DateTime(2007, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/unvhwxrfzpjys4txfv4a.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/wworjqefsfzc9ouvrpxd.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/kjbwbdqemykovdzgidhu.jpg\"]", "BioShock is a horror-themed first-person shooter set in a steampunk underwater dystopia. The player is urged to turn everything into a weapon: biologically modifying their own body with Plasmids, hacking devices and systems, upgrading their weapons, crafting new ammo variants, and experimenting with different battle techniques are all possible. The game is described by the developers as a spiritual successor to their previous PC title System Shock 2. BioShock received high praise in critical reviews for its atmospheric audio and visual quality, absorbing and original plot and its unique gaming experience.", "https://www.youtube.com/embed/CoYorK3E4aM" },
                    { 71L, 71L, 86.778606969433241, "//images.igdb.com/igdb/image/upload/t_cover_big/co1x7d.jpg", true, "Portal", 0, new DateTime(2007, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/x7pzfljardlljvtqcgv4.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/co2mtfe04a5iclqoo11c.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/oycbiasvmiewjxncqlgd.jpg\"]", "Waking up in a seemingly empty laboratory, the player is made to complete various physics-based puzzle challenges through numerous test chambers in order to test out the new Aperture Science Handheld Portal Device, without an explanation as to how, why or by whom.", "https://www.youtube.com/embed/nA9ChSA6wV4" },
                    { 72L, 72L, 91.849080624078795, "//images.igdb.com/igdb/image/upload/t_cover_big/co1rs4.jpg", true, "Portal 2", 0, new DateTime(2011, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/i9ys3zdhph1mh3futdit.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/rvrge8js7xnhr4z1vrbk.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/cmxaa4r52exlqvzwtxkh.jpg\"]", "Sequel to the acclaimed Portal (2007), Portal 2 pits the protagonist of the original game, Chell, and her new robot friend, Wheatley, against more puzzles conceived by GLaDOS, an A.I. with the sole purpose of testing the Portal Gun's mechanics and taking revenge on Chell for the events of Portal. As a result of several interactions and revelations, Chell once again pushes to escape Aperture Science Labs.", "https://www.youtube.com/embed/mC_u9ZwlIUc" },
                    { 127L, 127L, 86.909118381031959, "//images.igdb.com/igdb/image/upload/t_cover_big/co1rcf.jpg", true, "Assassin's Creed II", 0, new DateTime(2009, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/wkagtolikjqalonaaixb.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/jbeteinfgsoxkoxv08i0.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/zvhehvzfakxb7aob8j7m.jpg\"]", "Discover an intriguing and epic story of power, revenge and conspiracy set during a pivotal moment in history: the Italian Renaissance.\nExperience the freedom and immersion of an all new open world and mission structure with settings such as the rooftops and canals of beautiful Venice. Your options in combat, assassination and escape are vast, with many new weapons, settings and gameplay elements.", "https://www.youtube.com/embed/TcuEqTzRXl4" },
                    { 233L, 233L, 90.65844625699026, "//images.igdb.com/igdb/image/upload/t_cover_big/co1nmw.jpg", true, "Half-Life 2", 0, new DateTime(2004, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/mpphkihhk8yh9m2zaafd.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/muyxb9cljgsy245fcimx.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/nnlfbf8blflrlmuarfej.jpg\"]", "1998. HALF-LIFE sends a shock through the game industry with its combination of pounding action and continuous, immersive storytelling.\n\nNOW. By taking the suspense, challenge and visceral charge of the original, and adding startling new realism and responsiveness, Half-Life 2 opens the door to a world where the player's presence affects everything around them, from the physical environment to the behaviors even the emotions of both friends and enemies.", "https://www.youtube.com/embed/ID1dWN3n7q4" },
                    { 472L, 472L, 87.692356473666834, "//images.igdb.com/igdb/image/upload/t_cover_big/co1tnw.jpg", true, "The Elder Scrolls V: Skyrim", 0, new DateTime(2011, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/muv70yw3rds1cw8ymr5v.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/xzk2h41fiye7uwbhc6ub.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/urqw7ltwmhr39gkidsih.jpg\"]", "Skyrim reimagines and revolutionizes the open-world fantasy epic, bringing to life a complete virtual world open for you to explore any way you choose. Play any type of character you can imagine, and do whatever you want; the legendary freedom of choice, storytelling, and adventure of The Elder Scrolls is realized like never before.", "https://www.youtube.com/embed/0mHGygvlKCQ" },
                    { 732L, 732L, 90.588774009660852, "//images.igdb.com/igdb/image/upload/t_cover_big/co2lb9.jpg", true, "Grand Theft Auto: San Andreas", 0, new DateTime(2004, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/sojdbdt93e06wojplpsj.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/hircn6ewsgu70ynlzis5.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/eswubyh9h3uereuyumjq.jpg\"]", "Returning after his mother's murder to the semi-fictional city of Los Santos (based on Los Angeles), Carl Johnson, a former gang banger, must take back the streets for his family and friends by gaining respect and once again gaining control over the streets. However, a story filled with crime, lies and corruption will lead him to trudge the entire state of San Andreas (based on California and Nevada) to rebuild his life.", "https://www.youtube.com/embed/vdlpWZpwOq0" },
                    { 1009L, 1009L, 93.376525089101833, "//images.igdb.com/igdb/image/upload/t_cover_big/co1r7f.jpg", true, "The Last of Us", 0, new DateTime(2013, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/upogjfthdffjlzfi26xe.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/emvrwg5vhpfcmn9loxgu.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/kdt90b2rbx4tmewaxur9.jpg\"]", "A third person shooter/stealth/survival hybrid, in which twenty years after the outbreak of a parasitic fungus which takes over the neural functions of humans, Joel, a Texan with a tragic familial past, finds himself responsible with smuggling a fourteen year old girl named Ellie to a militia group called the Fireflies, while avoiding strict and deadly authorities, infected fungal hosts and other violent survivors.", "https://www.youtube.com/embed/fxeNaDjU7sw" },
                    { 1020L, 1020L, 89.82090131285085, "//images.igdb.com/igdb/image/upload/t_cover_big/co2lbd.jpg", true, "Grand Theft Auto V", 0, new DateTime(2013, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/o7q3ikzmkjxbftrd64ok.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/vfdeo6kgu0o4cyzd0sng.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/eepecmqsq6uqxiaukar1.jpg\"]", "Grand Theft Auto V is a vast open world game set in Los Santos, a sprawling sun-soaked metropolis struggling to stay afloat in an era of economic uncertainty and cheap reality TV. The game blends storytelling and gameplay in new ways as players repeatedly jump in and out of the lives of the game’s three lead characters, playing all sides of the game’s interwoven story.", "https://www.youtube.com/embed/QkkoHAzjnUs" },
                    { 1942L, 1942L, 94.145552782745355, "//images.igdb.com/igdb/image/upload/t_cover_big/co1wyy.jpg", true, "The Witcher 3: Wild Hunt", 0, new DateTime(2015, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/mnljdjtrh44x4snmierh.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/em1y2ugcwy2myuhvb9db.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/usxccsncekxg0wd1v6ee.jpg\"]", "RPG and sequel to The Witcher 2 (2011), The Witcher 3 follows witcher Geralt of Rivia as he seeks out his former lover and his young subject while intermingling with the political workings of the wartorn Northern Kingdoms. Geralt has to fight monsters and deal with people of all sorts in order to solve complex problems and settle contentious disputes, each ranging from the personal to the world-changing.", "https://www.youtube.com/embed/5nLipy-Z4yo" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "GenreName", "IgdbGenreId" },
                values: new object[,]
                {
                    { 2L, "Point-and-click", 2L },
                    { 4L, "Fighting", 4L },
                    { 5L, "Shooter", 5L },
                    { 7L, "Music", 7L },
                    { 8L, "Platform", 8L },
                    { 9L, "Puzzle", 9L },
                    { 10L, "Racing", 10L },
                    { 11L, "Real Time Strategy (RTS)", 11L },
                    { 12L, "Role-playing (RPG)", 12L },
                    { 13L, "Simulator", 13L },
                    { 14L, "Sport", 14L },
                    { 15L, "Strategy", 15L },
                    { 16L, "Turn-based strategy (TBS)", 16L },
                    { 24L, "Tactical", 24L },
                    { 25L, "Hack and slash/Beat 'em up", 25L },
                    { 26L, "Quiz/Trivia", 26L },
                    { 30L, "Pinball", 30L },
                    { 31L, "Adventure", 31L },
                    { 32L, "Indie", 32L },
                    { 33L, "Arcade", 33L },
                    { 34L, "Visual Novel", 34L },
                    { 35L, "Card & Board Game", 35L },
                    { 36L, "MOBA", 36L }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "PlatformId", "IgdbPlatformId", "PlatformName" },
                values: new object[,]
                {
                    { 3L, 3L, "Linux" },
                    { 6L, 6L, "PC (Microsoft Windows)" },
                    { 8L, 8L, "PlayStation 2" },
                    { 9L, 9L, "PlayStation 3" },
                    { 11L, 11L, "Xbox" },
                    { 39L, 39L, "iOS" },
                    { 49L, 49L, "Xbox One" },
                    { 130L, 130L, "Nintendo Switch" },
                    { 167L, 167L, "PlayStation 5" },
                    { 169L, 169L, "Xbox Series X|S" }
                });

            migrationBuilder.InsertData(
                table: "GameCompany",
                columns: new[] { "IgdbCompanyId", "IgdbId" },
                values: new object[,]
                {
                    { 8L, 20L },
                    { 13L, 20L },
                    { 1L, 71L },
                    { 56L, 71L },
                    { 1L, 72L },
                    { 56L, 72L },
                    { 38L, 127L },
                    { 104L, 127L },
                    { 24L, 233L },
                    { 56L, 233L },
                    { 126L, 472L },
                    { 29L, 732L },
                    { 29L, 1020L },
                    { 50L, 1942L }
                });

            migrationBuilder.InsertData(
                table: "GameGenre",
                columns: new[] { "IgdbGenreId", "IgdbId" },
                values: new object[,]
                {
                    { 5L, 20L },
                    { 9L, 20L },
                    { 12L, 20L },
                    { 31L, 20L },
                    { 5L, 71L },
                    { 8L, 71L },
                    { 9L, 71L },
                    { 5L, 72L },
                    { 8L, 72L },
                    { 9L, 72L },
                    { 31L, 72L },
                    { 8L, 127L },
                    { 31L, 127L },
                    { 5L, 233L },
                    { 12L, 472L },
                    { 31L, 472L },
                    { 5L, 732L },
                    { 10L, 732L },
                    { 31L, 732L },
                    { 5L, 1009L },
                    { 31L, 1009L },
                    { 5L, 1020L },
                    { 10L, 1020L },
                    { 31L, 1020L },
                    { 12L, 1942L },
                    { 31L, 1942L }
                });

            migrationBuilder.InsertData(
                table: "GamePlatform",
                columns: new[] { "IgdbId", "IgdbPlatformId" },
                values: new object[,]
                {
                    { 20L, 6L },
                    { 20L, 9L },
                    { 71L, 3L },
                    { 71L, 6L },
                    { 71L, 9L },
                    { 71L, 130L },
                    { 72L, 3L },
                    { 72L, 6L },
                    { 72L, 9L },
                    { 72L, 130L },
                    { 127L, 6L },
                    { 127L, 9L },
                    { 127L, 39L },
                    { 233L, 3L },
                    { 233L, 6L },
                    { 233L, 9L },
                    { 233L, 11L },
                    { 472L, 6L },
                    { 472L, 9L },
                    { 732L, 6L },
                    { 732L, 8L },
                    { 732L, 9L },
                    { 732L, 11L },
                    { 732L, 39L },
                    { 1009L, 9L },
                    { 1020L, 6L },
                    { 1020L, 9L },
                    { 1020L, 49L },
                    { 1942L, 6L },
                    { 1942L, 49L },
                    { 1942L, 130L },
                    { 1942L, 167L },
                    { 1942L, 169L }
                });
        }
    }
}
