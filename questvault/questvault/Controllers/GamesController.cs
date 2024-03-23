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
                var filteredGames = await _context.Games
                    .OrderByDescending(o => o.IgdbRating)
                    .ThenByDescending(o => o.TotalRatingCount)
                    .ToListAsync();

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
            ViewBag.SearchTerm = searchTerm;
            // Se o searchTerm for nulo, retorne NotFound
            if (searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
            var results = await _context.Games.Where(e => e.Name.Contains(searchTerm))
                .OrderByDescending(o => o.IgdbRating)
                .ThenByDescending(o => o.TotalRatingCount)
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
           
            if (searchTerm == null)
            {
                return RedirectToAction("Index");
            }

            var games = await _igdbService.SearchGames(searchTerm);
            if (games == null)
            {
                return NotFound();
            }

            var companyIds = games.SelectMany(g => g.GameCompanies.Select(c => c.IgdbCompanyId)).Distinct().ToList();
            var platformIds = games.SelectMany(g => g.GamePlatforms.Select(p => p.IgdbPlatformId)).Distinct().ToList();
            var genresIds = games.SelectMany(g => g.GameGenres.Select(gg => gg.IgdbGenreId)).Distinct().ToList();

            var existingCompanies = await _context.Companies.Where(c => companyIds.Contains(c.IgdbCompanyId)).ToListAsync();
            var existingPlatforms = await _context.Platforms.Where(p => platformIds.Contains(p.IgdbPlatformId)).ToListAsync();
            var existingGenres = await    _context.Genres.Where(g => genresIds.Contains(g.IgdbGenreId)).ToListAsync();

            bool allCompaniesExist = existingCompanies.Count == companyIds.Count;
            bool allPlatformsExist = existingPlatforms.Count == platformIds.Count;
            bool allGenresExist = existingGenres.Count == genresIds.Count;

            if (!allCompaniesExist)
            {
                // Buscar empresas ausentes na API IGDB e adicionar à lista de empresas
                var missingCompanies = await _igdbService.GetCompaniesFromIds(
                    companyIds.Except(existingCompanies.Select(c => c.IgdbCompanyId)).ToList());
                
                foreach (var company in missingCompanies)
                {
                    if (!existingCompanies.Any(c => c.IgdbCompanyId == company.IgdbCompanyId))
                    {
                        var newComp = new Models.Company
                        {
                            IgdbCompanyId = company.IgdbCompanyId,
                            CompanyName = company.CompanyName,
                        };
                        _context.Companies.Add(newComp);

                        existingCompanies.Add(newComp);
                    }
                }
            }

            if(!allPlatformsExist)
            {
                var missingPlatforms = await _igdbService.GetPlatformsFromIds(
                    platformIds.Except(existingPlatforms.Select(c => c.IgdbPlatformId)).ToList());

                foreach (var platform in missingPlatforms)
                {
                    if (!existingPlatforms.Any(p => p.IgdbPlatformId == platform.IgdbPlatformId))
                    {
                        var newPlat = new Models.Platform
                        {
                            IgdbPlatformId = platform.IgdbPlatformId,
                            PlatformName = platform.PlatformName,
                        };
                        _context.Platforms.Add(newPlat);
                        existingPlatforms.Add(newPlat);
                    }
                }
            }

            if (!allGenresExist)
            {
                var missingGenres = await _igdbService.GetGenresFromIds(
                    genresIds.Except(existingGenres.Select(gg => gg.IgdbGenreId)).ToList());

                foreach (var genre in missingGenres)
                {
                    if (!existingGenres.Any(g => g.IgdbGenreId == genre.IgdbGenreId))
                    {
                        var newGenre = new Models.Genre
                        {
                            IgdbGenreId = genre.IgdbGenreId,
                            GenreName = genre.GenreName
                        };
                        _context.Genres.Add(newGenre);
                        existingGenres.Add(newGenre);
                    }
                }

            }
            foreach (var game in games)
            {
                var existingGame = await _context.Games.FirstOrDefaultAsync(g => g.IgdbId == game.IgdbId);

                if (existingGame == null)
                {
                    var newGame = new Models.Game
                    {
                        IgdbId = game.IgdbId,
                        Name = game.Name,
                        Summary = game.Summary,
                        IgdbRating = game.IgdbRating,
                        TotalRatingCount = game.TotalRatingCount,
                        ImageUrl = game.ImageUrl,
                        Screenshots = game.Screenshots,
                        VideoUrl = game.VideoUrl,
                        QvRating = game.QvRating,
                        ReleaseDate = game.ReleaseDate,
                        IsReleased = game.IsReleased,
                    };

                    _context.Games.Add(newGame);

                    foreach (var company in existingCompanies)
                    {
                        var companyBelongsToGame = game.GameCompanies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);
                        
                        if (companyBelongsToGame != null)
                        {
                            _context.GameCompany.Add(new GameCompany
                            {
                                Game = newGame,
                                Company = company
                            });
                        }
                    }

                    foreach (var genre in existingGenres)
                    {
                        var existingGenre = game.GameGenres.FirstOrDefault(g => g.IgdbGenreId == genre.IgdbGenreId);
                        if (existingGenre != null)
                        {
                            _context.GameGenre.Add(new GameGenre
                            {
                                Game = newGame,
                                Genre = genre
                            });
                        }
                    }

                    foreach (var platform in existingPlatforms)
                    {
                        var existingPlatform = game.GamePlatforms.FirstOrDefault(p => p.IgdbPlatformId == platform.IgdbPlatformId);
                        if (existingPlatform != null)
                        {
                            _context.GamePlatform.Add(new GamePlatform
                            {
                                Game = newGame,
                                Platform = platform
                            });
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Results", new { searchTerm = searchTerm });
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

            var gameLog = userLibrary != null ? userLibrary.GameLogs.FirstOrDefault(g => g.IgdbId == id) : null;

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



    }
}
