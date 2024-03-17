using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class scg2173 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeactivated = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    Clearance = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IgdbId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IgdbRating = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Screenshots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QvRating = table.Column<double>(type: "float", nullable: false),
                    ReleaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    PlatformId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.PlatformId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTokens", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_EmailTokens_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameCompany",
                columns: table => new
                {
                    IgdbId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCompany", x => new { x.IgdbId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_GameCompany_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCompany_Games_IgdbId",
                        column: x => x.IgdbId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    IgdbId = table.Column<long>(type: "bigint", nullable: false),
                    GenreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.IgdbId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenre_Games_IgdbId",
                        column: x => x.IgdbId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    IgdbId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.IgdbId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_GamePlatform_Games_IgdbId",
                        column: x => x.IgdbId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "PlatformId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyName" },
                values: new object[,]
                {
                    { 1L, "Electronic Arts" },
                    { 8L, "2K Games" },
                    { 13L, "Demiurge Studios" },
                    { 24L, "Sierra Entertainment" },
                    { 29L, "Rockstar Games" },
                    { 38L, "Ubisoft Montreal" },
                    { 50L, "WB Games" },
                    { 56L, "Valve" },
                    { 104L, "Ubisoft Entertainment" },
                    { 126L, "Bethesda Game Studios" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "IgdbId", "IgdbRating", "ImageUrl", "Name", "QvRating", "ReleaseDate", "Screenshots", "Summary", "VideoUrl" },
                values: new object[,]
                {
                    { 20L, 20L, 86.738029894909729, "//images.igdb.com/igdb/image/upload/t_cover_big/co2mli.jpg", "BioShock", 0.0, "August 21, 2007", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/unvhwxrfzpjys4txfv4a.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/wworjqefsfzc9ouvrpxd.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/kjbwbdqemykovdzgidhu.jpg\"]", "BioShock is a horror-themed first-person shooter set in a steampunk underwater dystopia. The player is urged to turn everything into a weapon: biologically modifying their own body with Plasmids, hacking devices and systems, upgrading their weapons, crafting new ammo variants, and experimenting with different battle techniques are all possible. The game is described by the developers as a spiritual successor to their previous PC title System Shock 2. BioShock received high praise in critical reviews for its atmospheric audio and visual quality, absorbing and original plot and its unique gaming experience.", "https://www.youtube.com/embed/CoYorK3E4aM" },
                    { 71L, 71L, 86.778517641759336, "//images.igdb.com/igdb/image/upload/t_cover_big/co1x7d.jpg", "Portal", 0.0, "October 10, 2007", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/x7pzfljardlljvtqcgv4.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/co2mtfe04a5iclqoo11c.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/oycbiasvmiewjxncqlgd.jpg\"]", "Waking up in a seemingly empty laboratory, the player is made to complete various physics-based puzzle challenges through numerous test chambers in order to test out the new Aperture Science Handheld Portal Device, without an explanation as to how, why or by whom.", "https://www.youtube.com/embed/nA9ChSA6wV4" },
                    { 72L, 72L, 91.84852424624583, "//images.igdb.com/igdb/image/upload/t_cover_big/co1rs4.jpg", "Portal 2", 0.0, "April 18, 2011", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/i9ys3zdhph1mh3futdit.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/rvrge8js7xnhr4z1vrbk.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/cmxaa4r52exlqvzwtxkh.jpg\"]", "Sequel to the acclaimed Portal (2007), Portal 2 pits the protagonist of the original game, Chell, and her new robot friend, Wheatley, against more puzzles conceived by GLaDOS, an A.I. with the sole purpose of testing the Portal Gun's mechanics and taking revenge on Chell for the events of Portal. As a result of several interactions and revelations, Chell once again pushes to escape Aperture Science Labs.", "https://www.youtube.com/embed/mC_u9ZwlIUc" },
                    { 127L, 127L, 86.909133880884752, "//images.igdb.com/igdb/image/upload/t_cover_big/co1rcf.jpg", "Assassin's Creed II", 0.0, "November 17, 2009", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/wkagtolikjqalonaaixb.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/jbeteinfgsoxkoxv08i0.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/zvhehvzfakxb7aob8j7m.jpg\"]", "Discover an intriguing and epic story of power, revenge and conspiracy set during a pivotal moment in history: the Italian Renaissance.\nExperience the freedom and immersion of an all new open world and mission structure with settings such as the rooftops and canals of beautiful Venice. Your options in combat, assassination and escape are vast, with many new weapons, settings and gameplay elements.", "https://www.youtube.com/embed/TcuEqTzRXl4" },
                    { 233L, 233L, 90.659012483195042, "//images.igdb.com/igdb/image/upload/t_cover_big/co1nmw.jpg", "Half-Life 2", 0.0, "November 16, 2004", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/mpphkihhk8yh9m2zaafd.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/muyxb9cljgsy245fcimx.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/nnlfbf8blflrlmuarfej.jpg\"]", "1998. HALF-LIFE sends a shock through the game industry with its combination of pounding action and continuous, immersive storytelling.\n\nNOW. By taking the suspense, challenge and visceral charge of the original, and adding startling new realism and responsiveness, Half-Life 2 opens the door to a world where the player's presence affects everything around them, from the physical environment to the behaviors even the emotions of both friends and enemies.", "https://www.youtube.com/embed/ID1dWN3n7q4" },
                    { 472L, 472L, 87.693564090508843, "//images.igdb.com/igdb/image/upload/t_cover_big/co1tnw.jpg", "The Elder Scrolls V: Skyrim", 0.0, "November 10, 2011", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/muv70yw3rds1cw8ymr5v.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/xzk2h41fiye7uwbhc6ub.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/urqw7ltwmhr39gkidsih.jpg\"]", "Skyrim reimagines and revolutionizes the open-world fantasy epic, bringing to life a complete virtual world open for you to explore any way you choose. Play any type of character you can imagine, and do whatever you want; the legendary freedom of choice, storytelling, and adventure of The Elder Scrolls is realized like never before.", "https://www.youtube.com/embed/0mHGygvlKCQ" },
                    { 732L, 732L, 90.589426824837233, "//images.igdb.com/igdb/image/upload/t_cover_big/co2lb9.jpg", "Grand Theft Auto: San Andreas", 0.0, "October 26, 2004", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/sojdbdt93e06wojplpsj.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/hircn6ewsgu70ynlzis5.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/eswubyh9h3uereuyumjq.jpg\"]", "Returning after his mother's murder to the semi-fictional city of Los Santos (based on Los Angeles), Carl Johnson, a former gang banger, must take back the streets for his family and friends by gaining respect and once again gaining control over the streets. However, a story filled with crime, lies and corruption will lead him to trudge the entire state of San Andreas (based on California and Nevada) to rebuild his life.", "https://www.youtube.com/embed/vdlpWZpwOq0" },
                    { 1009L, 1009L, 93.378166330256974, "//images.igdb.com/igdb/image/upload/t_cover_big/co1r7f.jpg", "The Last of Us", 0.0, "June 14, 2013", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/upogjfthdffjlzfi26xe.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/emvrwg5vhpfcmn9loxgu.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/kdt90b2rbx4tmewaxur9.jpg\"]", "A third person shooter/stealth/survival hybrid, in which twenty years after the outbreak of a parasitic fungus which takes over the neural functions of humans, Joel, a Texan with a tragic familial past, finds himself responsible with smuggling a fourteen year old girl named Ellie to a militia group called the Fireflies, while avoiding strict and deadly authorities, infected fungal hosts and other violent survivors.", "https://www.youtube.com/embed/fxeNaDjU7sw" },
                    { 1020L, 1020L, 89.821145750184655, "//images.igdb.com/igdb/image/upload/t_cover_big/co2lbd.jpg", "Grand Theft Auto V", 0.0, "September 17, 2013", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/o7q3ikzmkjxbftrd64ok.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/vfdeo6kgu0o4cyzd0sng.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/eepecmqsq6uqxiaukar1.jpg\"]", "Grand Theft Auto V is a vast open world game set in Los Santos, a sprawling sun-soaked metropolis struggling to stay afloat in an era of economic uncertainty and cheap reality TV. The game blends storytelling and gameplay in new ways as players repeatedly jump in and out of the lives of the game’s three lead characters, playing all sides of the game’s interwoven story.", "https://www.youtube.com/embed/QkkoHAzjnUs" },
                    { 1942L, 1942L, 94.145487751664035, "//images.igdb.com/igdb/image/upload/t_cover_big/co1wyy.jpg", "The Witcher 3: Wild Hunt", 0.0, "May 19, 2015", "[\"//images.igdb.com/igdb/image/upload/t_screenshot_med/mnljdjtrh44x4snmierh.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/em1y2ugcwy2myuhvb9db.jpg\",\"//images.igdb.com/igdb/image/upload/t_screenshot_med/usxccsncekxg0wd1v6ee.jpg\"]", "RPG and sequel to The Witcher 2 (2011), The Witcher 3 follows witcher Geralt of Rivia as he seeks out his former lover and his young subject while intermingling with the political workings of the wartorn Northern Kingdoms. Geralt has to fight monsters and deal with people of all sorts in order to solve complex problems and settle contentious disputes, each ranging from the personal to the world-changing.", "https://www.youtube.com/embed/5nLipy-Z4yo" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "GenreName" },
                values: new object[,]
                {
                    { 2L, "Point-and-click" },
                    { 4L, "Fighting" },
                    { 5L, "Shooter" },
                    { 7L, "Music" },
                    { 8L, "Platform" },
                    { 9L, "Puzzle" },
                    { 10L, "Racing" },
                    { 11L, "Real Time Strategy (RTS)" },
                    { 12L, "Role-playing (RPG)" },
                    { 13L, "Simulator" },
                    { 14L, "Sport" },
                    { 15L, "Strategy" },
                    { 16L, "Turn-based strategy (TBS)" },
                    { 24L, "Tactical" },
                    { 25L, "Hack and slash/Beat 'em up" },
                    { 26L, "Quiz/Trivia" },
                    { 30L, "Pinball" },
                    { 31L, "Adventure" },
                    { 32L, "Indie" },
                    { 33L, "Arcade" },
                    { 34L, "Visual Novel" },
                    { 35L, "Card & Board Game" },
                    { 36L, "MOBA" }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "PlatformId", "PlatformName" },
                values: new object[,]
                {
                    { 3L, "Linux" },
                    { 6L, "PC (Microsoft Windows)" },
                    { 8L, "PlayStation 2" },
                    { 9L, "PlayStation 3" },
                    { 11L, "Xbox" },
                    { 39L, "iOS" },
                    { 49L, "Xbox One" },
                    { 130L, "Nintendo Switch" },
                    { 167L, "PlayStation 5" },
                    { 169L, "Xbox Series X|S" }
                });

            migrationBuilder.InsertData(
                table: "GameCompany",
                columns: new[] { "CompanyId", "IgdbId" },
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
                columns: new[] { "GenreId", "IgdbId" },
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
                columns: new[] { "IgdbId", "PlatformId" },
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTokens_UserId1",
                table: "EmailTokens",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameCompany_CompanyId",
                table: "GameCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenreId",
                table: "GameGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformId",
                table: "GamePlatform",
                column: "PlatformId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmailTokens");

            migrationBuilder.DropTable(
                name: "GameCompany");

            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
