using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
//using questvault.Migrations;
using questvault.Services;
using questvault.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace questvault.Controllers
{
  /// <summary>
  /// Controller for managing games.
  /// </summary>
  /// <remarks>
  /// Constructor for GamesController.
  /// </remarks>
  /// <param name="context">The application's database context.</param>
  /// <param name="igdbService">The IGDB service for game-related operations.</param>
  [Route("[controller]")]
  public class GamesController(
          ApplicationDbContext context,
          IServiceIGDB igdbService,
          SignInManager<User> signInManager
        ) : Controller
  {
    private readonly int _pageSize = 20;

    /// <summary>
    /// Index of the page Games that shows all the games and the filtered ones.
    /// </summary>
    /// <param name="releaseStatus">The term to search for the status release.</param>
    /// <param name="genre">The term to search for genre.</param>
    /// <param name="releasePlatform">The term to search for platform.</param>
    /// <returns>A view with the games data</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(int? pageNumber, string releaseStatus, string genre, string releasePlatform)
    {
      // Defina as variáveis de exibição para armazenar os valores selecionados
      ViewBag.SelectedReleaseStatus = releaseStatus;
      ViewBag.SelectedGenre = genre;
      ViewBag.SelectedReleasePlatform = releasePlatform;
      //ViewBag.SelectedReleaseYear = releaseYear;
      if (genre == null && releasePlatform == null && releaseStatus == null)
      {
        var filteredGames = context.Games
            .OrderByDescending(o => o.IgdbRating)
            .ThenByDescending(o => o.TotalRatingCount);

        var list = await PaginatedList<Game>.CreateAsync(filteredGames.AsNoTracking(), pageNumber ?? 1, _pageSize);
        var data = new GameViewData
        {
          NumberOfResults = list.Count,
          Games = list,
          Genres = context.Genres,
          Platforms = context.Platforms.Distinct(),
        };
        return View(data);
      }
      else
      {
        var query = context.Games.AsQueryable();

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

        //var filteredGames = await query.OrderByDescending(g => g.IgdbRating).ToListAsync();
        var list = await PaginatedList<Game>.CreateAsync(query.AsNoTracking().OrderByDescending(g => g.IgdbRating), pageNumber ?? 1, _pageSize);
        var data = new GameViewData
        {
          NumberOfResults = list.Count,
          Games = list,
          Genres = context.Genres.Distinct(),
          Platforms = context.Platforms.Distinct(),
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
    [Authorize]
    [HttpGet]
    [Route("results")]
    public async Task<IActionResult> Results(string searchTerm, int? pageNumber)
    {
      ViewBag.SearchTerm = searchTerm;
      // Se o searchTerm for nulo, retorne NotFound
      if (searchTerm == null)
      {
                await Console.Out.WriteLineAsync("searchTerm is null");
                
                return RedirectToAction("Index");
      }

      // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
      var query = context.Games.Where(e => e.Name.Contains(searchTerm))
          .OrderByDescending(o => o.IgdbRating)
          .ThenByDescending(o => o.TotalRatingCount);
      ViewBag.NumberOfResults = query.Count();
      var list = await PaginatedList<Game>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, _pageSize);
      var data = new GameViewData
      {
        SearchTerm = searchTerm,
        NumberOfResults = list.Count,
        Games = list,
        Genres = context.Genres,
        Platforms = context.Platforms,
      };
      return View(data);
    }

    private static async Task<int> SaveChangesAsync(ApplicationDbContext context)
    {
      try
      {
        return await context.SaveChangesAsync();
      }
      catch (Exception e)
      {
        if (e is DbUpdateConcurrencyException || e is DbUpdateException)
        {
          await Console.Out.WriteLineAsync("Data already in the db");
          return 0;
        }
        else
        {
          await Console.Out.WriteLineAsync($"Unexpected exception: {e.Message}");
          return -1;
        }
      }
    }

    /// <summary>
    /// Post (saves in database) Results with the games based on a search term.
    /// </summary>
    /// <param name="searchTerm">The term to search for.</param>
    /// <returns>A view and a collection of games matching the search term sorted by rating.</returns>
    [Authorize]
    [HttpPost]
    [Route("results")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResultsPost(string searchTerm)
    {

      if (searchTerm == null)
      {
        return RedirectToAction("Index");
      }

      var games = await igdbService.SearchGames(searchTerm);
      if (games == null)
      {
        return NotFound();
      }

      var companyIds = games.SelectMany(g => g.GameCompanies.Select(c => c.IgdbCompanyId)).Distinct().ToList();
      var platformIds = games.SelectMany(g => g.GamePlatforms.Select(p => p.IgdbPlatformId)).Distinct().ToList();
      var genresIds = games.SelectMany(g => g.GameGenres.Select(gg => gg.IgdbGenreId)).Distinct().ToList();

      var existingCompanies = await context.Companies.Where(c => companyIds.Contains(c.IgdbCompanyId)).ToListAsync();
      var existingPlatforms = await context.Platforms.Where(p => platformIds.Contains(p.IgdbPlatformId)).ToListAsync();
      var existingGenres = await context.Genres.Where(g => genresIds.Contains(g.IgdbGenreId)).ToListAsync();

      bool allCompaniesExist = existingCompanies.Count == companyIds.Count;
      bool allPlatformsExist = existingPlatforms.Count == platformIds.Count;
      bool allGenresExist = existingGenres.Count == genresIds.Count;
      using (var transaction = context.Database.BeginTransaction())
      {
        try
        {
          // Seu código de busca e inserção aqui...

          // Commit da transação se tudo correr bem
          if (!allCompaniesExist)
          {

            var missingCompanies = await igdbService.GetCompaniesFromIds(
                companyIds.Except(existingCompanies.Select(c => c.IgdbCompanyId)).ToList());

            foreach (var company in missingCompanies)
            {
              var existingCompany = await context.Companies.FirstOrDefaultAsync(g => g.IgdbCompanyId == company.IgdbCompanyId);
              if (existingCompany == null)
              {
                var newComp = new Models.Company
                {
                  IgdbCompanyId = company.IgdbCompanyId,
                  CompanyName = company.CompanyName,
                };
                await context.Companies.AddAsync(newComp);

                existingCompanies.Add(newComp);
                await SaveChangesAsync(context);
              }
            }
          }
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          //await Console.Out.WriteLineAsync(ex.Message);
          // Rollback da transação em caso de erro
          await transaction.RollbackAsync();
          // Tratar ou registrar o erro conforme necessário
          return RedirectToAction("Error", "Home");
        }

      }

      using (var transaction = context.Database.BeginTransaction())
      {
        try
        {
          // Seu código de busca e inserção aqui...
          if (!allPlatformsExist)
          {
            var missingPlatforms = await igdbService.GetPlatformsFromIds(
                platformIds.Except(existingPlatforms.Select(c => c.IgdbPlatformId)).ToList());

            foreach (var platform in missingPlatforms)
            {
              var existingPlatform = await context.Platforms.FirstOrDefaultAsync(g => g.IgdbPlatformId == platform.IgdbPlatformId);
              if (existingPlatform == null)
              {
                var newPlat = new Models.Platform
                {
                  IgdbPlatformId = platform.IgdbPlatformId,
                  PlatformName = platform.PlatformName,
                };
                await context.Platforms.AddAsync(newPlat);
                existingPlatforms.Add(newPlat);
                await SaveChangesAsync(context);
              }
            }
          }

          // Commit da transação se tudo correr bem
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          await Console.Out.WriteLineAsync(ex.Message);
          // Rollback da transação em caso de erro
          await transaction.RollbackAsync();
          // Tratar ou registrar o erro conforme necessário
          return RedirectToAction("Error", "Home");
        }
      }

      using (var transaction = context.Database.BeginTransaction())
      {
        try
        {
          // Seu código de busca e inserção aqui...
          if (!allGenresExist)
          {
            var missingGenres = await igdbService.GetGenresFromIds(
                genresIds.Except(existingGenres.Select(gg => gg.IgdbGenreId)).ToList());

            foreach (var genre in missingGenres)
            {
              var existingGenre = await context.Genres.FirstOrDefaultAsync(g => g.IgdbGenreId == genre.IgdbGenreId);
              if (existingGenre == null)
              {
                var newGenre = new Models.Genre
                {
                  IgdbGenreId = genre.IgdbGenreId,
                  GenreName = genre.GenreName
                };
                await context.Genres.AddAsync(newGenre);
                existingGenres.Add(newGenre);
                await SaveChangesAsync(context);
              }
            }

          }

          // Commit da transação se tudo correr bem
          await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
          // Rollback da transação em caso de erro
          await transaction.RollbackAsync();
          // Tratar ou registrar o erro conforme necessário
          return RedirectToAction("Error", "Home");
        }
      }
      foreach (var game in games)
      {
        var existingGame = await context.Games.FirstOrDefaultAsync(g => g.IgdbId == game.IgdbId);

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

          context.Games.Add(newGame);
          foreach (var company in existingCompanies)
          {
            var companyBelongsToGame = game.GameCompanies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);

            if (companyBelongsToGame != null)
            {
              context.GameCompany.Add(new GameCompany
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
              context.GameGenre.Add(new GameGenre
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
              context.GamePlatform.Add(new GamePlatform
              {
                Game = newGame,
                Platform = platform
              });
            }
          }
          await SaveChangesAsync(context);
        }
      }

      return RedirectToAction("Results", new { searchTerm });
    }


    /// <summary>
    /// Retrieves details of a game with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the game.</param>
    /// <returns>An asynchronous task representing the action result.</returns>
    [Authorize]
    [HttpGet]
    [Route("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
      

      if (context.Games == null)
      {
        return NotFound();
      }

      var game = await context.Games
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

      var user = await signInManager.UserManager.GetUserAsync(User);

      var userLibrary = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .FirstOrDefaultAsync(g => g.User == user);

      bool isGameAddedToLibrary = userLibrary != null && userLibrary.GameLogs.Any(g => g.IgdbId == id);

      var gameLog = userLibrary?.GameLogs.FirstOrDefault(g => g.IgdbId == id);

      if (gameLog != null)
      {
        ViewBag.GamelogId = gameLog.GameLogId;
        ViewBag.Status = gameLog.Status;
        ViewBag.Review = gameLog.Review;
        ViewBag.Rating = gameLog.Rating;
      }

      ViewBag.IsGameAddedToLibrary = isGameAddedToLibrary;

      ViewBag.IsGameTop5 =
        userLibrary != null &&
        userLibrary.Top5Games != null &&
        userLibrary.Top5Games.Any(g => g.IgdbId == id);

      ViewBag.Reviews = GetReviews(id, userLibrary);
      await Console.Out.WriteLineAsync("Reviews:");
      foreach (var review in ViewBag.Reviews)
        await Console.Out.WriteLineAsync($"game: {review.Game.Name}, user: {review.User.UserName}, rating: {review.Rating}, review: {review.Review}");
      return View(game);
    }
           


    private List<GameLog> GetReviews(int? gameId, GamesLibrary currentUserLibrary)
    {
      if (gameId == null) return [];
      return [.. context.GameLog
        .Include(gl => gl.Game).Include(gl => gl.User)
        .Where(gl => gl.IgdbId == gameId && gl.Rating != null)];
    }

    /// <summary>
    /// Gets the games based on a search term.(used in search bar for autocomplete function)
    /// </summary>
    /// <param name="searchTerm">The term to search for.</param>
    /// <returns>Json data with games information.</returns>
    [Authorize]
    [HttpGet]
    [Route("search")]
    public IActionResult Search(string searchTerm)
    {
      if (searchTerm == null || context.Games == null)
      {
        return NotFound();
      }

      var games = context.Games
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
    private void ProcessGameGenres(Game game, Game newGame)
    {
      foreach (var genre in game.GameGenres)
      {
        var existingGenre = context.Genres.FirstOrDefault(c => c.IgdbGenreId == genre.IgdbGenreId);
        if (existingGenre != null)
        {
          // Create a new GameGenre object
          var gameGenre = new GameGenre
          {
            Game = newGame,
            Genre = existingGenre
          };

          // Add the GameGenre to the context
          context.GameGenre.Add(gameGenre);
        }
      }
    }
  }
}