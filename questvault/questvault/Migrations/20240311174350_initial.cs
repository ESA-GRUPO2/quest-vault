using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace questvault.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                name: "Company",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreID);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    PlatformID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.PlatformID);
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
                name: "Games",
                columns: table => new
                {
                    GameID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: true),
                    PlatformID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameID);
                    table.ForeignKey(
                        name: "FK_Games_Company_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Company",
                        principalColumn: "CompanyID");
                    table.ForeignKey(
                        name: "FK_Games_Platform_PlatformID",
                        column: x => x.PlatformID,
                        principalTable: "Platform",
                        principalColumn: "PlatformID");
                });

            migrationBuilder.CreateTable(
                name: "GamesGenres",
                columns: table => new
                {
                    GamesID = table.Column<int>(type: "int", nullable: false),
                    GenresID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesGenres", x => new { x.GamesID, x.GenresID });
                    table.ForeignKey(
                        name: "FK_GamesGenres_Games_GamesID",
                        column: x => x.GamesID,
                        principalTable: "Games",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamesGenres_Genres_GenresID",
                        column: x => x.GenresID,
                        principalTable: "Genres",
                        principalColumn: "GenreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameID", "CompanyID", "Name", "PlatformID", "Rating", "Summary" },
                values: new object[,]
                {
                    { 15, null, "Fallout 3", null, 83.527103920909681, "Fallout 3 from the creators of the award-winning Oblivion, featuring one of the most realized game worlds ever created. Create any kind of character you want and explore the open wastes of post-apocalyptic Washington D.C. Every minute is a fight for survival as you encounter Super Mutants, Ghouls, Raiders and other dangers of the Wasteland. Prepare for the future.\n\nThe third game in the Fallout series, Fallout 3 is a singleplayer action role-playing game (RPG) set in a post-apocalyptic Washington DC. Combining the horrific insanity of the Cold War era theory of mutually assured destruction gone terribly wrong, with the kitschy naivety of American 1950s nuclear propaganda, Fallout 3 will satisfy both players familiar with the popular first two games in its series as well as those coming to the franchise for the first time." },
                    { 16, null, "Fallout: New Vegas", null, 87.019966534345784, "In this first-person Western RPG, the player takes on the role of Courier 6, barely surviving after being robbed of their cargo, shot and put into a shallow grave by a New Vegas mob boss. The Courier sets out to track down their robbers and retrieve their cargo, and winds up getting tangled in the complex ideological and socioeconomic web of the many factions and settlements of post-nuclear Nevada." },
                    { 18, null, "Max Payne", null, 85.965926921874683, "Max Payne is a man with nothing to lose in the violent, cold urban night. A fugitive undercover cop framed for murder, hunted by cops and the mob, Max is a man with his back against the wall, fighting a battle he cannot hope to win. Max Payne is a relentless story-driven game about a man on the edge, fighting to clear his name while struggling to uncover the truth about his slain family amongst a myriad of plot-twists and twisted thugs in the gritty bowels of New York during the century's worst blizzard.\n\nThe groundbreaking original cinematic action-shooter, Max Payne introduced the concept of Bullet Time in videogames. Through its stylish slow-motion gunplay combined with a dark and twisted story, Max Payne redefined the action-shooter genre." },
                    { 20, null, "BioShock", null, 86.739828401883614, "BioShock is a horror-themed first-person shooter set in a steampunk underwater dystopia. The player is urged to turn everything into a weapon: biologically modifying their own body with Plasmids, hacking devices and systems, upgrading their weapons, crafting new ammo variants, and experimenting with different battle techniques are all possible. The game is described by the developers as a spiritual successor to their previous PC title System Shock 2. BioShock received high praise in critical reviews for its atmospheric audio and visual quality, absorbing and original plot and its unique gaming experience." },
                    { 71, null, "Portal", null, 86.777775518353337, "Waking up in a seemingly empty laboratory, the player is made to complete various physics-based puzzle challenges through numerous test chambers in order to test out the new Aperture Science Handheld Portal Device, without an explanation as to how, why or by whom." },
                    { 72, null, "Portal 2", null, 91.850130392615029, "Sequel to the acclaimed Portal (2007), Portal 2 pits the protagonist of the original game, Chell, and her new robot friend, Wheatley, against more puzzles conceived by GLaDOS, an A.I. with the sole purpose of testing the Portal Gun's mechanics and taking revenge on Chell for the events of Portal. As a result of several interactions and revelations, Chell once again pushes to escape Aperture Science Labs." },
                    { 73, null, "Mass Effect", null, 86.295074852037885, "What starts as a routine mission to an agrarian outpost quickly becomes the opening salvo in an epic war. As the newly appointed Executive Officer of the SSV Normandy, you'll assemble and lead an elite squad of heroes into battle after heart-pounding battle. Each decision you make will impact not only your fate, but the destiny of the entire galaxy in the Mass Effect trilogy.\n\nKey Features:\n\nIncredible, interactive storytelling. Create and customize your own character, from Commander Shepard's appearance and skills to a personalized arsenal. Unleash devastating abilities as you command and train. Your decisions will control the outcome of each mission, your relationships with your crew and ultimately the entire war.\n\nAn amazing universe to explore. From the massive Citadel to the harsh, radioactive landscape of the Krogan home world – the incredible breadth of the Mass Effect universe will blow you away. Travel to the farthest outposts aboard the SSV Normandy, the most technologically advanced ship in the galaxy. You'll follow the clues left by ancient civilizations, discover hidden bases with fantastic new tech and lead your hand-picked crew into explosive alien battles.\n\nEdge-of-your-seat excitement meets strategic combat. Find the perfect combination of squad-mates and weapons for each battle if you want to lead them to victory. Sun-Tzu's advice remains as pertinent in 2183 as it is today – know your enemy. You'll need different tactics for a squad of enemies with devastating biotic attacks than a heavily armored Geth Colossus so choose your teams wisely." },
                    { 74, null, "Mass Effect 2", null, 91.338660325704438, "Are you prepared to lose everything to save the galaxy? You'll need to be, Commander Shephard. It's time to bring together your greatest allies and recruit the galaxy's fighting elite to continue the resistance against the invading Reapers. So steel yourself, because this is an astronomical mission where sacrifices must be made. You'll face tougher choices and new, deadlier enemies. Arm yourself and prepare for an unforgettable intergalactic adventure.\n\nGame Features:\n\nShift the fight in your favour. Equip yourself with powerful new weapons almost instantly thanks to a new inventory system. Plus, an improved health regeneration system means you'll spend less time hunting for restorative items.\n\nMake every decision matter. Divisive crew members are just the tip of the iceberg, Commander, because you'll also be tasked with issues of intergalactic diplomacy. And time's a wastin' so don't be afraid to use new prompt-based actions that let you interrupt conversations, even if they could alter the fate of your crew...and the galaxy.\n\nForge new alliances, carefully. You'll fight alongside some of your most trustworthy crew members, but you'll also get the opportunity to recruit new talent. Just choose your new partners with care because the fate of the galaxy rests on your shoulders, Commander." },
                    { 75, null, "Mass Effect 3", null, 85.17530971927863, "Earth is burning. The Reapers have taken over and other civilizations are falling like dominoes. Lead the final fight to save humanity and take back Earth from these terrifying machines, Commander Shepard. You'll need backup for these battles. Fortunately, the galaxy has a habit of sending unexpected species your way. Recruit team members and forge new alliances, but be prepared to say goodbye at any time as partners make the ultimate sacrifice. It's time for Commander Shepard to fight for the fate of the human race and save the galaxy. No pressure, Commander.\n\nFight smarter. Take advantage of new powers and combat moves. Shepard can now blind fire at enemies and build tougher melee attacks. Plus, when you fight as a team you can combine new biotic and tech powers to unleash devastating Power Combos.\n\nBuild the final force. Build a team from a variety of races and classes and combine their skills to overcome impossible odds. You'll be joined by newcomers like James Vega, a tough-as-nails soldier, as well as EDI, a trusted AI in a newly acquired physical form. Keep an eye out for beloved characters from your past, but beware. Some may not survive the final battle...\n\nFace off against friends. Enjoy an integrated co-op multiplayer mode and team up with friends online to liberate key conflict zones from increasingly tough opponents. Customize your warrior and earn new weapons, armor, and abilities to build war preparedness stats in your single player campaign." },
                    { 76, null, "Dragon Age: Origins", null, 86.44866918060977, "Dragon Age: Origins is a third-person role-playing game described as a spiritual successor to Baldur's Gate and Neverwinter Nights franchises. Players create their own character by customizing gender and appearance as well as choosing a race and class. Combat is in real time with the ability to pause at any moment: tactical options include an editor which allows the player to give the AI detailed instructions on how to behave in every possible situation. Although the main storyline is the same for every character created, the game features six unique prologues, two for each race. Dragon Age: Origins received critical acclaim upon release, with praise mostly directed at its story, setting, characters, music and combat system." },
                    { 113, null, "Assassin's Creed Brotherhood", null, 82.853936686042431, "Live and breathe as Ezio, a legendary Master Assassin, in his enduring struggle against the powerful Templar Order. He must journey into Italy’s greatest city, Rome, center of power, greed and corruption to strike at the heart of the enemy.\nDefeating the corrupt tyrants entrenched there will require not only strength, but leadership, as Ezio commands an entire Brotherhood who will rally to his side. Only by working together can the Assassins defeat their mortal enemies.\nAnd for the first time, introducing a never-before-seen multiplayer layer that allows you to choose from a wide range of unique characters, each with their own signature weapons and assassination techniques, and match your skills against other players from around the world.\nIt’s time to join the Brotherhood." },
                    { 121, null, "Minecraft", null, 84.033979625235503, "Minecraft focuses on allowing the player to explore, interact with, and modify a dynamically-generated map made of one-cubic-meter-sized blocks. In addition to blocks, the environment features plants, mobs, and items. Some activities in the game include mining for ore, fighting hostile mobs, and crafting new blocks and tools by gathering various resources found in the game. The game's open-ended model allows players to create structures, creations, and artwork on various multiplayer servers or their single-player maps. Other features include redstone circuits for logic computations and remote actions, minecarts and tracks, and a mysterious underworld called the Nether. A designated but completely optional goal of the game is to travel to a dimension called the End, and defeat the ender dragon." },
                    { 127, null, "Assassin's Creed II", null, 86.917321771555009, "Discover an intriguing and epic story of power, revenge and conspiracy set during a pivotal moment in history: the Italian Renaissance.\nExperience the freedom and immersion of an all new open world and mission structure with settings such as the rooftops and canals of beautiful Venice. Your options in combat, assassination and escape are vast, with many new weapons, settings and gameplay elements." },
                    { 128, null, "Assassin's Creed", null, 73.271366521090584, "Assassin's Creed is a non-linear action-adventure video game, during which the player controls a 12th-century Levantine Assassin named Altaïr Ibn-La'Ahad during the Third Crusade, whose life is experienced through the Animus by his 21st century descendant, Desmond Miles." },
                    { 231, null, "Half-Life", null, 88.785089339294046, "Dr. Gordon Freeman doesn't speak, but he's got a helluva story to tell. This first-person roller-coaster initiated a new era in the history of action games by combining engrossing gameplay, upgraded graphics, ingenious level design and a revolutionary story that may not be all that it seems, told not through cutscenes, but through the visual environment." },
                    { 233, null, "Half-Life 2", null, 90.659032917457338, "1998. HALF-LIFE sends a shock through the game industry with its combination of pounding action and continuous, immersive storytelling.\n\nNOW. By taking the suspense, challenge and visceral charge of the original, and adding startling new realism and responsiveness, Half-Life 2 opens the door to a world where the player's presence affects everything around them, from the physical environment to the behaviors even the emotions of both friends and enemies." },
                    { 434, null, "Red Dead Redemption", null, 89.340103940291641, "A modern-day Western epic, Red Dead Redemption takes John Marston, a relic from the fast-closing time of the gunslinger, through an open-world filled with wildlife, mini games and shootouts. Marston sets out to hunt down his old gang mates for the government, who have taken away his family, and meets many characters emblematic of the Wild West, heroism and the new civilization along his journey." },
                    { 472, null, "The Elder Scrolls V: Skyrim", null, 87.69369540690036, "Skyrim reimagines and revolutionizes the open-world fantasy epic, bringing to life a complete virtual world open for you to explore any way you choose. Play any type of character you can imagine, and do whatever you want; the legendary freedom of choice, storytelling, and adventure of The Elder Scrolls is realized like never before." },
                    { 500, null, "Batman: Arkham Asylum", null, 85.921804117204573, "Using a great variety of gadgets you must make your way around the island, and the asylums halls to find and stop the joker. The game uses a 3-button combat system, but with a great number of gadget abilites which Batman can unlock. This makes for a very cinematic combat experience when fighting the Joker's goons." },
                    { 501, null, "Batman: Arkham City", null, 87.261028920393471, "Batman: Arkham City builds upon the intense, atmospheric foundation of Batman: Arkham Asylum, sending players soaring into Arkham City, the new maximum security \"home\" for all of Gotham City's thugs, gangsters and insane criminal masterminds.\n\nSet inside the heavily fortified walls of a sprawling district in the heart of Gotham City, this highly anticipated sequel introduces a brand-new story that draws together a new all-star cast of classic characters and murderous villains from the Batman universe, as well as a vast range of new and enhanced gameplay features to deliver the ultimate experience as the Dark Knight." },
                    { 529, null, "Far Cry 3", null, 83.732348718391108, "Beyond the reach of civilization lies a lawless island ruled by violence. This is where you find yourself stranded, caught in a bloody conflict between the island’s psychotic warlords and indigenous rebels. Struggling to survive, your only hope of escape is through the muzzle of a gun. Discover the island’s dark secrets and take the fight to the enemy; improvise and use your environment to your advantage; and outwit its cast of ruthless, deranged inhabitants. Beware the beauty and mystery of this island of insanity… Where nothing is what is seems, you’ll need more than luck to escape alive." },
                    { 533, null, "Dishonored", null, 84.874780945462049, "Dishonored is an immersive first-person action game that casts you as a supernatural assassin driven by revenge. With Dishonored’s flexible combat system, creatively eliminate your targets as you combine the supernatural abilities, weapons and unusual gadgets at your disposal. Pursue your enemies under the cover of darkness or ruthlessly attack them head on with weapons drawn. The outcome of each mission plays out based on the choices you make." },
                    { 538, null, "BioShock Infinite", null, 85.802211114776952, "BioShock Infinite is the third game in the BioShock series. It is not a direct sequel/prequel to any of the previous BioShock games but takes place in an entirely different setting, although it shares similar features, gameplay and concepts with the previous games. BioShock Infinite features a range of environments that force the player to adapt, with different weapons and strategies for each situation. Interior spaces feature close combat with enemies, but unlike previous games set in Rapture, the setting of Infinite contains open spaces with emphasis on sniping and ranged combat against as many as fifteen enemies at once." },
                    { 565, null, "Uncharted 2: Among Thieves", null, 88.96713707224167, "In the sequel to Drake's Fortune, Nathan Drake comes across a map that showcases the location of Marco Polo's missing ships. It takes him on a journey to find the infamous Cintamani Stone, and uncover the truth behind it." },
                    { 623, null, "Call of Duty 4: Modern Warfare", null, 84.413485983002815, "Call of Duty 4: Modern Warfare differs from previous installments of the Call of Duty series. Previous Call of Duty games have a distinct three country-specific campaign style, while Call of Duty 4 has a more film-like plot with interlaced story lines from the perspectives of Sgt. Paul Jackson of the Marines 1st Force Recon and Sgt. 'Soap' MacTavish of the British 22nd SAS Regiment." },
                    { 731, null, "Grand Theft Auto IV", null, 83.571344737558988, "Grand Theft Auto IV is an action-adventure video game developed by Rockstar North and published by Rockstar Games. It is the eleventh title in the Grand Theft Auto series, and the first main entry since 2004's Grand Theft Auto: San Andreas.\n\nThe game is played from a third-person perspective and its world is navigated on-foot or by vehicle. Throughout the single-player mode, players play as Niko Bellic. An online multiplayer mode is included with the game, allowing up to 32 players to engage in both co-operative and competitive gameplay in a recreation of the single-player setting.\n\nTwo expansion packs were later released for the game, The Lost and Damned and The Ballad of Gay Tony, which both feature new plots that are interconnected with the main Grand Theft Auto IV storyline, and follow new protagonists." },
                    { 732, null, "Grand Theft Auto: San Andreas", null, 90.589305216088775, "Returning after his mother's murder to the semi-fictional city of Los Santos (based on Los Angeles), Carl Johnson, a former gang banger, must take back the streets for his family and friends by gaining respect and once again gaining control over the streets. However, a story filled with crime, lies and corruption will lead him to trudge the entire state of San Andreas (based on California and Nevada) to rebuild his life." },
                    { 733, null, "Grand Theft Auto: Vice City", null, 87.668782476460905, "In the year 1986, Tommy Vercetti is heavily indebted to his mafia superiors after a drug deal gone awry, but his dreams of taking over Vice City (based on Miami) push him down a different path. Featuring a wide variety of vehicles and weapons, radio stations playing hit songs from the era and an intense atmosphere, GTA: Vice City is an open-world sandbox satire of '80's Miami." },
                    { 1009, null, "The Last of Us", null, 93.352240521857155, "A third person shooter/stealth/survival hybrid, in which twenty years after the outbreak of a parasitic fungus which takes over the neural functions of humans, Joel, a Texan with a tragic familial past, finds himself responsible with smuggling a fourteen year old girl named Ellie to a militia group called the Fireflies, while avoiding strict and deadly authorities, infected fungal hosts and other violent survivors." },
                    { 1011, null, "Borderlands 2", null, 84.33165391631124, "A new era of shoot and loot is about to begin. Play as one of four new vault hunters facing off against a massive new world of creatures, psychos and the evil mastermind, Handsome Jack. Make new friends, arm them with a bazillion weapons and fight alongside them in 4 player co-op on a relentless quest for revenge and redemption across the undiscovered and unpredictable living planet." },
                    { 1020, null, "Grand Theft Auto V", null, 89.820437867083484, "Grand Theft Auto V is a vast open world game set in Los Santos, a sprawling sun-soaked metropolis struggling to stay afloat in an era of economic uncertainty and cheap reality TV. The game blends storytelling and gameplay in new ways as players repeatedly jump in and out of the lives of the game’s three lead characters, playing all sides of the game’s interwoven story." },
                    { 1029, null, "The Legend of Zelda: Ocarina of Time", null, 92.158555628900444, "The Legend of Zelda: Ocarina of Time is the fifth main installment of The Legend of Zelda series and the first to be released for the Nintendo 64. It was one of the most highly anticipated games of its age, and is listed among the greatest video games ever created by numerous websites and magazines. The gameplay of Ocarina of Time was revolutionary for its time, it has arguably made more of an impact on later games in the series than any of its predecessors even though they had the same cores of exploration, dungeons, puzzles and item usage. Among the gameplay mechanics, one of the most noteworthy is the time-traveling system. The game begins with the player controlling the child Link, but later on an adult Link becomes a playable character as well and each of them has certain unique abilities. Ocarina of Time also introduces the use of music to solve puzzles: as new songs are learned, they can be used to solve puzzles, gain access to new areas and warp to different locations. Dungeon exploration is somewhat more puzzle-oriented than in earlier games but they are not too complex." },
                    { 1070, null, "Super Mario World", null, 92.607371113631487, "A 2D platformer and first entry on the SNES in the Super Mario franchise, Super Mario World follows Mario as he attempts to defeat Bowser's underlings and rescue Princess Peach from his clutches. The game features a save system, a less linear world map, an expanded movement arsenal and numerous new items for Mario, alongside new approaches to level design and art direction." },
                    { 1074, null, "Super Mario 64", null, 89.692847874733076, "The first three dimensional entry in the Mario franchise, Super Mario 64 follows Mario as he puts his broadened 3D movement arsenal to use in order to rescue Princess Peach from the clutches of his arch rival Bowser. Mario has to jump into worlds-within-paintings ornamenting the walls of Peach's castle, uncover secrets and hidden challenges and collect golden stars as reward for platforming trials." },
                    { 1164, null, "Tomb Raider", null, 81.960577501446267, "Tomb Raider explores the intense and gritty origin story of Lara Croft and her ascent from a young woman to a hardened survivor. Armed only with raw instincts and the ability to push beyond the limits of human endurance, Lara must fight to unravel the dark history of a forgotten island to escape its relentless hold." },
                    { 1871, null, "The Walking Dead", null, 85.667994505549899, "The Walking Dead: Season One (also known as The Walking Dead: The Game) is an episodic interactive drama graphic adventure video game developed and published by Telltale Games. Based on Robert Kirkman's The Walking Dead comic book series, the game consists of five episodes, released between April and November 2012. It is available for Android, iOS, Kindle Fire HDX, Microsoft Windows, Mac OS X, PlayStation 3, PlayStation Vita, Xbox 360, PlayStation 4 and Xbox One. The game is the first of The Walking Dead video game series published by Telltale." },
                    { 1942, null, "The Witcher 3: Wild Hunt", null, 94.145586486610682, "RPG and sequel to The Witcher 2 (2011), The Witcher 3 follows witcher Geralt of Rivia as he seeks out his former lover and his young subject while intermingling with the political workings of the wartorn Northern Kingdoms. Geralt has to fight monsters and deal with people of all sorts in order to solve complex problems and settle contentious disputes, each ranging from the personal to the world-changing." },
                    { 1970, null, "Assassin's Creed IV Black Flag", null, 82.938127887579839, "Assassin’s Creed IV Black Flag begins in 1715, when pirates established a lawless republic in the Caribbean and ruled the land and seas. These outlaws paralyzed navies, halted international trade, and plundered vast fortunes. They threatened the power structures that ruled Europe, inspired the imaginations of millions, and left a legacy that still endures." },
                    { 7331, null, "Uncharted 4: A Thief's End", null, 90.962547055165686, "Several years after his last adventure, retired fortune hunter, Nathan Drake, is forced back into the world of thieves. With the stakes much more personal, Drake embarks on a globe-trotting journey in pursuit of a historical conspiracy behind a fabled pirate treasure. His greatest adventure will test his physical limits, his resolve, and ultimately what he's willing to sacrifice to save the ones he loves." },
                    { 7342, null, "Inside", null, 86.113629232474921, "An atmospheric 2D side-scroller in which, hunted and alone, a boy finds himself drawn into the center of a dark project and struggles to preserve his identity." },
                    { 7346, null, "The Legend of Zelda: Breath of the Wild", null, 92.797742524418041, "The Legend of Zelda: Breath of the Wild is the first 3D open-world game in the Zelda series. Link can travel anywhere and be equipped with weapons and armor found throughout the world to grant him various bonuses. Unlike many games in the series, Breath of the Wild does not impose a specific order in which quests or dungeons must be completed. While the game still has environmental obstacles such as weather effects, inhospitable lands, or powerful enemies, many of them can be overcome using the right method. A lot of critics ranked Breath of the Wild as one of the best video games of all time." },
                    { 7351, null, "Doom", null, 85.544041820657952, "Developed by id software, the studio that pioneered the first-person shooter genre and created multiplayer Deathmatch, Doom returns as a brutally fun and challenging modern-day shooter experience. Relentless demons, impossibly destructive guns, and fast, fluid movement provide the foundation for intense, first-person combat – whether you’re obliterating demon hordes through the depths of Hell in the single-player campaign, or competing against your friends in numerous multiplayer modes. Expand your gameplay experience using Doom SnapMap game editor to easily create, play, and share your content with the world." },
                    { 7599, null, "Life is Strange", null, 82.870118543267765, "Life is Strange is a five part episodic game that sets out to revolutionize story based choice and consequence games by allowing the player to rewind time and affect the past, present and future." },
                    { 9630, null, "Fallout 4", null, 79.984937502201859, "Bethesda Game Studios welcome you to the world of Fallout 4, their most ambitious game ever, and the next generation of open-world gaming. As the sole survivor of Vault 111, you enter a world destroyed by nuclear war. Every second is a fight for survival, and every choice is yours. Only you can rebuild and determine the fate of the Wasteland. Welcome home." },
                    { 11156, null, "Horizon Zero Dawn", null, 88.145144028010534, "Welcome to a vibrant world rich with the beauty of nature – but inhabited by awe-inspiring, highly advanced machines. As a young machine hunter named Aloy, you must unravel the mysteries of this world and find your own destiny." },
                    { 12517, null, "Undertale", null, 86.571986902300068, "A small child falls into the Underground, where monsters have long been banished by humans and are hunting every human that they find. The player controls the child as they try to make it back to the Surface through hostile environments, all the while engaging with a turn-based combat system with puzzle-solving and bullet hell elements, as well as other unconventional game mechanics." },
                    { 19560, null, "God of War", null, 93.497378259827357, "God of War is the sequel to God of War III as well as a continuation of the canon God of War chronology. Unlike previous installments, this game focuses on Norse mythology and follows an older and more seasoned Kratos and his son Atreus in the years since the third game. It is in this harsh, unforgiving world that he must fight to survive… and teach his son to do the same." },
                    { 19565, null, "Marvel's Spider-Man", null, 87.171760660369827, "Starring the world’s most iconic Super Hero, Spider-Man features the acrobatic abilities, improvisation and web-slinging that the wall-crawler is famous for, while also introducing elements never-before-seen in a Spider-Man game. From traversing with parkour and utilizing the environment, to new combat and blockbuster set pieces, it’s Spider-Man unlike any you’ve played before." },
                    { 25076, null, "Red Dead Redemption 2", null, 92.518490744523689, "Red Dead Redemption 2 is the epic tale of outlaw Arthur Morgan and the infamous Van der Linde gang, on the run across America at the dawn of the modern age." },
                    { 26758, null, "Super Mario Odyssey", null, 89.574919840990475, "Explore incredible places far from the Mushroom Kingdom as you join Mario and his new ally Cappy on a massive, globe-trotting 3D adventure. Use amazing new abilities, like the power to capture and control objects, animals, and enemies to collect Power Moons so you can power up the Odyssey airship and save Princess Peach from Bowser’s wedding plans!" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreID", "GenreName" },
                values: new object[,]
                {
                    { 2, "Point-and-click" },
                    { 4, "Fighting" },
                    { 5, "Shooter" },
                    { 7, "Music" },
                    { 8, "Platform" },
                    { 9, "Puzzle" },
                    { 10, "Racing" },
                    { 11, "Real Time Strategy (RTS)" },
                    { 12, "Role-playing (RPG)" },
                    { 13, "Simulator" },
                    { 14, "Sport" },
                    { 15, "Strategy" },
                    { 16, "Turn-based strategy (TBS)" },
                    { 24, "Tactical" },
                    { 25, "Hack and slash/Beat 'em up" },
                    { 26, "Quiz/Trivia" },
                    { 30, "Pinball" },
                    { 31, "Adventure" },
                    { 32, "Indie" },
                    { 33, "Arcade" },
                    { 34, "Visual Novel" },
                    { 35, "Card & Board Game" },
                    { 36, "MOBA" }
                });

            migrationBuilder.InsertData(
                table: "GamesGenres",
                columns: new[] { "GamesID", "GenresID" },
                values: new object[,]
                {
                    { 15, 5 },
                    { 15, 12 },
                    { 16, 5 },
                    { 16, 12 },
                    { 18, 5 },
                    { 20, 5 },
                    { 20, 9 },
                    { 20, 12 },
                    { 20, 31 },
                    { 71, 5 },
                    { 71, 8 },
                    { 71, 9 },
                    { 72, 5 },
                    { 72, 8 },
                    { 72, 9 },
                    { 72, 31 },
                    { 73, 5 },
                    { 73, 12 },
                    { 74, 5 },
                    { 74, 12 },
                    { 74, 31 },
                    { 75, 5 },
                    { 75, 12 },
                    { 75, 31 },
                    { 76, 12 },
                    { 113, 9 },
                    { 113, 31 },
                    { 121, 13 },
                    { 121, 31 },
                    { 127, 8 },
                    { 127, 31 },
                    { 128, 8 },
                    { 128, 31 },
                    { 231, 5 },
                    { 231, 9 },
                    { 231, 31 },
                    { 233, 5 },
                    { 434, 5 },
                    { 434, 12 },
                    { 434, 31 },
                    { 472, 12 },
                    { 472, 31 },
                    { 500, 25 },
                    { 500, 31 },
                    { 501, 25 },
                    { 501, 31 },
                    { 529, 5 },
                    { 529, 31 },
                    { 533, 9 },
                    { 533, 12 },
                    { 533, 31 },
                    { 538, 5 },
                    { 538, 31 },
                    { 565, 5 },
                    { 565, 8 },
                    { 565, 31 },
                    { 623, 5 },
                    { 623, 13 },
                    { 731, 5 },
                    { 731, 10 },
                    { 731, 31 },
                    { 732, 5 },
                    { 732, 10 },
                    { 732, 31 },
                    { 733, 5 },
                    { 733, 10 },
                    { 733, 31 },
                    { 733, 33 },
                    { 1009, 5 },
                    { 1009, 31 },
                    { 1011, 5 },
                    { 1011, 12 },
                    { 1020, 5 },
                    { 1020, 10 },
                    { 1020, 31 },
                    { 1029, 12 },
                    { 1029, 31 },
                    { 1070, 8 },
                    { 1070, 31 },
                    { 1074, 8 },
                    { 1074, 31 },
                    { 1164, 5 },
                    { 1164, 8 },
                    { 1164, 9 },
                    { 1164, 31 },
                    { 1871, 2 },
                    { 1871, 31 },
                    { 1942, 12 },
                    { 1942, 31 },
                    { 1970, 8 },
                    { 1970, 31 },
                    { 7331, 5 },
                    { 7331, 31 },
                    { 7342, 8 },
                    { 7342, 9 },
                    { 7342, 31 },
                    { 7342, 32 },
                    { 7346, 9 },
                    { 7346, 12 },
                    { 7346, 31 },
                    { 7351, 5 },
                    { 7351, 9 },
                    { 7599, 9 },
                    { 7599, 12 },
                    { 7599, 31 },
                    { 7599, 32 },
                    { 9630, 5 },
                    { 9630, 12 },
                    { 11156, 5 },
                    { 11156, 12 },
                    { 11156, 31 },
                    { 12517, 12 },
                    { 12517, 16 },
                    { 12517, 31 },
                    { 12517, 32 },
                    { 19560, 12 },
                    { 19560, 25 },
                    { 19560, 31 },
                    { 19565, 25 },
                    { 19565, 31 },
                    { 25076, 5 },
                    { 25076, 12 },
                    { 25076, 31 },
                    { 26758, 8 },
                    { 26758, 31 }
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
                name: "IX_Games_CompanyID",
                table: "Games",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlatformID",
                table: "Games",
                column: "PlatformID");

            migrationBuilder.CreateIndex(
                name: "IX_GamesGenres_GenresID",
                table: "GamesGenres",
                column: "GenresID");
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
                name: "GamesGenres");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Platform");
        }
    }
}
