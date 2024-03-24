using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using questvault.Data;
using questvault.Services;
using questvault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questvaultTest
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }
        private IServiceIGDB igdbService;
        public ApplicationDbContextFixture()
        {
            igdbService = new IGDBService("uzhx4rrftyohllg1mrpy3ajo7090q5", "7rvcth933kxra92ddery5qn3jxwap7");
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();

            var games = new List<Game>
            {
                new Game
                {
                    IgdbId = 99L,
                    IgdbRating = 86.734274201256369,
                    ImageUrl = "//images.igdb.com/igdb/image/upload/t_cover_big/co2mli.jpg",
                    IsReleased = false,
                    Name = "Game 1",
                    QvRating = 0,
                    ReleaseDate = new DateTime(2007, 8, 21),
                    Screenshots = new [] {
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/unvhwxrfzpjys4txfv4a.jpg",
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/wworjqefsfzc9ouvrpxd.jpg",
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/kjbwbdqemykovdzgidhu.jpg"
                    },
                    Summary = "BioShock is a horror-themed first-person shooter set in a steampunk underwater dystopia. The player is urged to turn everything into a weapon: biologically modifying their own body with Plasmids, hacking devices and systems, upgrading their weapons, crafting new ammo variants, and experimenting with different battle techniques are all possible. The game is described by the developers as a spiritual successor to their previous PC title System Shock 2. BioShock received high praise in critical reviews for its atmospheric audio and visual quality, absorbing and original plot and its unique gaming experience.",
                    VideoUrl = "https://www.youtube.com/embed/CoYorK3E4aM"
                },
                new Game
                {
                    IgdbId = 999L,
                    IgdbRating = 86.778367407367497,
                    ImageUrl = "//images.igdb.com/igdb/image/upload/t_cover_big/co1x7d.jpg",
                    IsReleased = true,
                    Name = "GAME 2",
                    QvRating = 0,
                    ReleaseDate = new DateTime(2007, 10, 10),
                    Screenshots = new [] {
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/x7pzfljardlljvtqcgv4.jpg",
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/co2mtfe04a5iclqoo11c.jpg",
                        "//images.igdb.com/igdb/image/upload/t_screenshot_med/oycbiasvmiewjxncqlgd.jpg"
                    },
                    Summary = "Waking up in a seemingly empty laboratory, the player is made to complete various physics-based puzzle challenges through numerous test chambers in order to test out the new Aperture Science Handheld Portal Device, without an explanation as to how, why or by whom.",
                    VideoUrl = "https://www.youtube.com/embed/nA9ChSA6wV4"
                },
                // Repita para os outros jogos
            };
            DbContext.Games.AddRange(games);
            DbContext.Users.AddRange(
                new User
                {
                    IsDeactivated = false,
                    IsPrivate = false,
                    Clearance = 0,
                    UserName = "UnitTester1",
                    NormalizedUserName = "UNITTESTER1",
                    Email = "unittester1@gmail.com",
                    NormalizedEmail = "UNITTESTER1@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = null,
                    SecurityStamp = null,
                    ConcurrencyStamp = null,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                },
                new User
                {
                    IsDeactivated = false,
                    IsPrivate = false,
                    Clearance = 0,
                    UserName = "UnitTester2",
                    NormalizedUserName = "UNITTESTER2",
                    Email = "unittester2@gmail.com",
                    NormalizedEmail = "UNITTESTER2@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = null,
                    SecurityStamp = null,
                    ConcurrencyStamp = null,
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                }
                );
            DbContext.SaveChanges();

        }




        public void Dispose() => DbContext.Dispose();
    }
}
