using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using IGDB;
using IGDB.Models;
//using questvault.Migrations;
using RestEase;
using questvault.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MailKit.Search;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MimeKit.Cryptography;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace questvault.Controllers
{
    /// <summary>
    /// Controller for managing games.
    /// Note: This controller is a work in progress (WIP) and may undergo significant changes.
    /// </summary>
    [Route("[controller]")]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceIGDB _igdbService;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Constructor for GamesController.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        /// <param name="igdbService">The IGDB service for game-related operations.</param>
        public GamesController(ApplicationDbContext context, IServiceIGDB igdbService, SignInManager<User> signInManager)
        {
            _context = context;
            _igdbService = igdbService;
            _signInManager = signInManager;
        }


        /// <summary>
        /// Index of the page Games that shows all the games and the filtered ones.
        /// </summary>
        /// <param name="releaseStatus">The term to search for the status release.</param>
        /// <param name="genre">The term to search for genre.</param>
        /// <param name="releasePlatform">The term to search for platform.</param>
        /// <returns>A view with the games data</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string releaseStatus, string genre, string releasePlatform)
        {
            // Defina as variáveis de exibição para armazenar os valores selecionados
            ViewBag.SelectedReleaseStatus = releaseStatus;
            ViewBag.SelectedGenre = genre;
            ViewBag.SelectedReleasePlatform = releasePlatform;
            //ViewBag.SelectedReleaseYear = releaseYear;
            if (genre == null && releasePlatform == null && releaseStatus == null)
            {
                var filteredGames = await _context.Games.OrderByDescending(
                    o => o.IgdbRating).ToListAsync();
                var data = new GameViewData
                {
                    NumberOfResults = filteredGames.Count,
                    Games = filteredGames,
                    Genres = _context.Genres,
                    Platforms = _context.Platforms.Distinct(),
                };
                return View(data);
            }
            else
            {
                var query = _context.Games.AsQueryable();

                if (!string.IsNullOrEmpty(releasePlatform))
                {
                    query = query.Where(g => g.GamePlatforms.Any(gp => gp.Platform.PlatformName.Equals(releasePlatform)));
                }

                if (!string.IsNullOrEmpty(genre))
                {
                    query = query.Where(g => g.GameGenres.Any(gg => gg.Genre.GenreName.Equals(genre)));
                }                
                if (!string.IsNullOrEmpty(releaseStatus))
                {
                    var released = (releaseStatus.Equals("released")) ? true : false;
                    query = query.Where(g => g.IsReleased == released);
                }
                //if (releaseYear > 0)
                //{
                //    query = query.Where(g => g.ReleaseDate.Value.Year == releaseYear);
                //}

                var filteredGames = await query.OrderByDescending(g => g.IgdbRating).ToListAsync();

                var data = new GameViewData
                {
                    NumberOfResults = filteredGames.Count,
                    Games = filteredGames,
                    Genres = _context.Genres,
                    Platforms = _context.Platforms.Distinct(),
                };
                return View(data);

            }
            // Processar os filtros recebidos e consultar o banco de dados

        }

        /// <summary>
        /// Gets the view Results with the games based on a search term.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>A view and a collection of games matching the search term sorted by rating.</returns>
        [HttpGet]
        [Route("results")]
        public async Task<IActionResult> Results(string searchTerm)
        {
            // Se o searchTerm for nulo, retorne NotFound
            if (searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
            var results = await _context.Games.Where(e => e.Name.Contains(searchTerm))
                .OrderByDescending(o => o.IgdbRating)
                .ToListAsync();
            var data = new GameViewData
            {
                SearchTerm = searchTerm,
                NumberOfResults = results.Count,
                Games = results,
                Genres = _context.Genres,
                Platforms = _context.Platforms,
            };
            return View(data);
        }
        /// <summary>
        /// Post (saves in database) Results with the games based on a search term.
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>A view and a collection of games matching the search term sorted by rating.</returns>
        [HttpPost]
        [Route("results")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResultsPost(string searchTerm)
        {
            // Check if search term is null
            if (searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            // Retrieve games from the IGDB service
            var games = await _igdbService.SearchGames(searchTerm);
            if (games == null)
            {
                return NotFound(); // TODO: TESTAR
            }

            // Process each game
            foreach (var game in games)
            {
                // Check if the game already exists in the database
                var existingGame = _context.Games.FirstOrDefault(g => g.IgdbId == game.IgdbId);
                if (existingGame == null)
                {
                    // Create a new game object
                    var newGame = new Models.Game
                    {
                        IgdbId = game.IgdbId,
                        Name = game.Name,
                        Summary = game.Summary,
                        IgdbRating = game.IgdbRating,
                        ImageUrl = game.ImageUrl,
                        Screenshots = game.Screenshots,
                        VideoUrl = game.VideoUrl,
                        QvRating = game.QvRating,
                        ReleaseDate = game.ReleaseDate,
                        IsReleased = game.IsReleased,
                    };

                    // Add the new game to the database
                    _context.Games.Add(newGame);

                    // Process game companies
                    await ProcessGameCompaniesAsync(game, newGame);

                    // Process game genres
                    ProcessGameGenres(game, newGame);

                    // Process game platforms
                    await ProcessGamePlatformsAsync(game, newGame);
                }
                await _context.SaveChangesAsync();
            }

            // Save changes to the database

            // Redirctec to action Results [GET] with searchTerm
            return RedirectToAction("Results", new { searchTerm = searchTerm });
        }

        /// <summary>
        /// Processes the game companies asynchronously and adds them 
        /// to the database and creates many to many relationship with game.
        /// </summary>
        /// <param name="game">The existing game be processed.</param>
        /// <param name="newGame">The new game to be added to GameCompany relationship.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessGameCompaniesAsync(Models.Game game, Models.Game newGame)
        {
            var companyIds = game.GameCompanies.Select(comp => comp.IgdbCompanyId).Distinct().ToList();
            var companies = await _igdbService.GetCompaniesFromIds(companyIds);

            foreach (var company in companies)
            {
                var existingCompany = _context.Companies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);
                if (existingCompany == null)
                {
                    // Create a new company object
                    var newCompany = new Models.Company
                    {
                        IgdbCompanyId = company.IgdbCompanyId,
                        CompanyName = company.CompanyName
                    };

                    // Add the new company to the database
                    _context.Companies.Add(newCompany);
                    existingCompany = newCompany;
                }

                // Create a new GameCompany object
                var gameCompany = new GameCompany
                {
                    Game = newGame,
                    Company = existingCompany
                };

                // Add the GameCompany to the context
                _context.GameCompany.Add(gameCompany);
            }
        }

        /// <summary>
        /// Processes the game genres and adds the new genres to the database and creates many to many relationship with game.
        /// </summary>
        /// <param name="game">The existing game be processed.</param>
        /// <param name="newGame">The new game to be added to GameGenre relationship.</param>
        private void ProcessGameGenres(Models.Game game, Models.Game newGame)
        {
            foreach (var genre in game.GameGenres)
            {
                var existingGenre = _context.Genres.FirstOrDefault(c => c.IgdbGenreId == genre.IgdbGenreId);
                if (existingGenre != null)
                {
                    // Create a new GameGenre object
                    var gameGenre = new GameGenre
                    {
                        Game = newGame,
                        Genre = existingGenre
                    };

                    // Add the GameGenre to the context
                    _context.GameGenre.Add(gameGenre);
                }
            }
        }

        /// <summary>
        /// Processes the game Platforms asynchronously and adds them 
        /// to the database and creates many to many relationship with game.
        /// </summary>
        /// <param name="game">The existing game be processed.</param>
        /// <param name="newGame">The new game to be added to GamePlatform relationship.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ProcessGamePlatformsAsync(Models.Game game, Models.Game newGame)
        {
            var platformIds = game.GamePlatforms.Select(pla => pla.IgdbPlatformId).Distinct().ToList();
            var platforms = await _igdbService.GetPlatformsFromIds(platformIds);

            foreach (var platform in platforms)
            {
                var existingPlatform = _context.Platforms.FirstOrDefault(p => p.IgdbPlatformId == platform.IgdbPlatformId);
                await Console.Out.WriteLineAsync("existing p= " + existingPlatform);
                if (existingPlatform == null)
                {
                    await Console.Out.WriteLineAsync("Doenst have this platform");
                    // Create a new platform object
                    var newPlatform = new Models.Platform
                    {
                        IgdbPlatformId = platform.IgdbPlatformId,
                        PlatformName = platform.PlatformName
                    };

                    // Add the new platform to the database
                    _context.Platforms.Add(newPlatform);
                    existingPlatform = newPlatform;
                }

                // Create a new GamePlatform object
                var gamePlatform = new GamePlatform
                {
                    Game = newGame,
                    Platform = existingPlatform
                };

                // Add the GamePlatform to the context
                _context.GamePlatform.Add(gamePlatform);
                
            }
        }



        /// <summary>
        /// Retrieves details of a game with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the game.</param>
        /// <returns>An asynchronous task representing the action result.</returns>
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound(); //TODO NOT FOUND
            }

            var game = await _context.Games
                .Include(g => g.GameGenres!)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms!)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameCompanies!)
                    .ThenInclude(gc => gc.Company)
                .FirstOrDefaultAsync(m => m.IgdbId == id);


            if (game == null)
            {
                return NotFound();
            }

            var user = await _signInManager.UserManager.GetUserAsync(User);

            var userLibrary = await _context.GamesLibrary
                .Include(g => g.GameLogs)
                .FirstOrDefaultAsync(g => g.User == user);

            bool isGameAddedToLibrary = userLibrary != null && userLibrary.GameLogs.Any(g => g.IgdbId == id);

            var gameLog = userLibrary.GameLogs.FirstOrDefault(g => g.IgdbId == id);

            if (gameLog != null)
            {
                ViewBag.Ownage = gameLog.Ownage;
                ViewBag.Status = gameLog.Status;
                ViewBag.Review = gameLog.Review;
                ViewBag.Rating = gameLog.Rating;
            }
            // Passe a variável para a visualização
            ViewBag.IsGameAddedToLibrary = isGameAddedToLibrary;


            return View(game);
        }

        /// <summary>
        /// Gets the games based on a search term.(used in search bar for autocomplete function)
        /// </summary>
        /// <param name="searchTerm">The term to search for.</param>
        /// <returns>Json data with games information.</returns>
        [HttpGet]
        [Route("search")]
        public IActionResult Search(string searchTerm)
        {
            if (searchTerm == null || _context.Games == null)
            {
                return NotFound();
            }

            var games = _context.Games
                .Where(g => EF.Functions.Like(g.Name, $"%{searchTerm}%"))
                .Select(s => new
                {
                    s.IgdbId,
                    s.Name,
                })
                .ToList();


            return Json(games);
        }

        //[HttpPost]
        //[Route ("results")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Results(string searchTerm)
        //{
        //    Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        //    if (searchTerm == null)
        //    {
        //        Console.WriteLine("primeiro if");
        //        return NotFound();
        //    }

        //    var games = await _igdbService.SearchGames(searchTerm);
        //    if (games == null)
        //    {
        //        Console.WriteLine("segundo if");
        //        return NotFound();
        //    }
        //    foreach (var game in games)
        //    {
        //        // Verifique se o jogo já existe na base de dados com base no GameId
        //        var existingGame = _context.Games.FirstOrDefault(g => g.IgdbId == game.IgdbId);
        //        await Console.Out.WriteLineAsync("exist?:-----> " + existingGame);
        //        // Se o jogo não existir, adicione-o à base de dados
        //        if (existingGame == null)
        //        {

        //            await Console.Out.WriteLineAsync("tem jogo!");
        //            // Crie um novo objeto Game com as propriedades desejadas
        //            var newGame = new Models.Game
        //            {
        //                IgdbId = game.IgdbId,
        //                Name = game.Name,
        //                Summary = game.Summary,
        //                IgdbRating = game.IgdbRating,
        //                ImageUrl = game.ImageUrl,
        //                Screenshots = game.Screenshots,
        //                VideoUrl = game.VideoUrl,
        //                QvRating = game.QvRating,
        //                ReleaseDate = game.ReleaseDate
        //            };
        //            Console.WriteLine("ad: "+  newGame.ToString());
        //            // Adicione o novo jogo à base de dados

        //            _context.Games.Add(newGame);
        //            //-----------------------------COMP----------------------------------------
        //            var idscomp = game.GameCompanies.Select(comp => comp.IgdbCompanyId).Distinct().ToList();
        //            var companies = _igdbService.GetCompaniesFromIds(idscomp).Result.ToList();
        //            foreach (var company in companies)
        //            {
        //                var existingCompany = _context.Companies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);
        //                if (existingCompany != null)
        //                {
        //                    // A empresa já existe, então associe-a ao jogo sem adicioná-la novamente
        //                    var compGameAdd = new GameCompany
        //                    {
        //                        Game = newGame,
        //                        Company = existingCompany
        //                    };
        //                    _context.GameCompany.Add(compGameAdd);
        //                }
        //                else
        //                {
        //                    // A empresa não existe, então adicione-a ao banco de dados e associe-a ao jogo
        //                    var newCompany = new Models.Company
        //                    {
        //                        IgdbCompanyId = company.IgdbCompanyId,
        //                        CompanyName = company.CompanyName,
        //                    };

        //                    _context.Companies.Add(newCompany);

        //                    var compGameAdd = new GameCompany
        //                    {
        //                        Game = newGame,
        //                        Company = newCompany
        //                    };
        //                    _context.GameCompany.Add(compGameAdd);
        //                }
        //            }

        //            //-----------------------------GEN----------------------------------
        //            foreach (var genre in game.GameGenres )
        //            {
        //                var existingGenre = _context.Genres.FirstOrDefault(c => c.IgdbGenreId == genre.IgdbGenreId);
        //                var gameGenreAdd = new GameGenre
        //                {
        //                    Game = newGame,
        //                    Genre = existingGenre
        //                };
        //                _context.GameGenre.Add(gameGenreAdd);
        //            }
        //            //-----------------------------PLATFORMS----------------------------------
        //            var idsplat = game.GamePlatforms.Select(pla => pla.IgdbPlatformId).Distinct().ToList();
        //            var platforms = _igdbService.GetPlatformsFromIds(idsplat).Result.ToList();
        //            foreach (var platform in platforms)
        //            {
        //                var existingPlatform = _context.Platforms.FirstOrDefault(c => c.IgdbPlatformId == platform.IgdbPlatformId);
        //                if (existingPlatform != null)
        //                {
        //                    // A empresa já existe, então associe-a ao jogo sem adicioná-la novamente
        //                    var platGameAdd = new GamePlatform
        //                    {
        //                        Game = newGame,
        //                        Platform = existingPlatform
        //                    };
        //                    _context.GamePlatform.Add(platGameAdd);
        //                }
        //                else
        //                {
        //                    // A empresa não existe, então adicione-a ao banco de dados e associe-a ao jogo
        //                    var newPlatform = new Models.Platform
        //                    {
        //                        IgdbPlatformId = platform.IgdbPlatformId,
        //                        PlatformName = platform.PlatformName,
        //                    };

        //                    _context.Platforms.Add(newPlatform);

        //                    var compGameAdd = new GamePlatform
        //                    {
        //                        Game = newGame,
        //                        Platform = newPlatform
        //                    };
        //                    _context.GamePlatform.Add(compGameAdd);
        //                }
        //            }
        //        }
        //    }
        //    // Salve as mudanças na base de dados
        //    await _context.SaveChangesAsync();
        //    //var allgames = await _context.Games
        //    //    .Where(g => EF.Functions.Like(g.Name, $"%{searchInput}%")).ToListAsync();
        //    //return View(allgames);
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Search([FromBody] string searchTerm)
        //{
        //    try
        //    {
        //        var jogos = await _igdbService.SearchGames(searchTerm);
        //        // Retorna os dados como JSON
        //        return Json(new { Jogos = jogos });
        //    }
        //    catch (ApiException ex)
        //    {
        //        Console.WriteLine(ex.Content);
        //        return Json(new { Erro = "Erro na API IGDB" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return Json(new { Erro = "Erro interno no servidor" });
        //    }
        //}

        //[HttpGet("Teste")]
        //public async Task<IActionResult> Popular()
        //{
        //    try
        //    {
        //        var jogos = await _igdbService.GetPopularGames(10);
        //        // Retorna os dados como JSON
        //        return Json(new { Jogos = jogos });
        //    }
        //    catch (ApiException ex)
        //    {
        //        Console.WriteLine(ex.Content);
        //        return Json(new { Erro = "Erro na API IGDB" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return Json(new { Erro = "Erro interno no servidor" });
        //    }
        //}

    }
}
