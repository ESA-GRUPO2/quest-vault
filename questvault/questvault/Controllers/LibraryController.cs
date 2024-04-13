using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;
using questvault.Utils;
using GameStatus = questvault.Models.GameStatus;

namespace questvault.Controllers
{
  public class LibraryController(
        ApplicationDbContext context,
        SignInManager<User> signInManager,
        SteamAPI steamAPI,
        IServiceIGDB igdbService
    ) : Controller
  {
    private readonly int _pageSize = 20;

    /// <summary>
    /// Action method for displaying the user's library.
    /// </summary>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpGet]
    [Route("UserLibrary")]
    public async Task<IActionResult> UserLibrary(string id, int? pageNumber, string? collection, string? releasePlatform, string? genre)
    {

      if( id == null )
      {
        ViewBag.Error = "Invalid user or game ID.";
        return NotFound();
      }
      var userLogged = await signInManager.UserManager.GetUserAsync(this.User);
      var user = await context.Users.Where(u => u.NormalizedUserName == id.ToUpper()).FirstAsync();

      if( user == null || userLogged == null )
      {
        ViewBag.Error = "Invalid user or game ID.";
        return NotFound();
      }
      bool friends = false;
      var friendships = await context.Friendship.ToListAsync();
      foreach( var friendship in friendships )
      {
        if( ( friendship.User1 == userLogged && friendship.User2 == user ) ||
            ( friendship.User1 == user && friendship.User2 == userLogged ) )
        {
          friends = true;
          break;
        }
      }

      if( user.IsPrivate && !friends && userLogged.Clearance == 0 && userLogged != user )
      {
        return RedirectToAction("PrivateProfile", "User", new { user.Id });
      }

      ViewBag.Collection = collection;
      ViewBag.SelectedGenre = genre;
      ViewBag.SelectedReleasePlatform = releasePlatform;
      ViewBag.UserLibraryId = user.UserName;

      if( String.IsNullOrEmpty(collection) )
      {

        var gamesInLibraryWithoutCollection = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == user)
        .SelectMany(gl => gl.GameLogs.Select(g => g.Game))
        ;

        if( !string.IsNullOrEmpty(releasePlatform) )
        {


          gamesInLibraryWithoutCollection = gamesInLibraryWithoutCollection.Where(g => g.GamePlatforms.Any(gp => gp.Platform.PlatformName.Equals(releasePlatform)));
        }

        if( !string.IsNullOrEmpty(genre) )
        {

          gamesInLibraryWithoutCollection = gamesInLibraryWithoutCollection.Where(g => g.GameGenres.Any(gg => gg.Genre.GenreName.Equals(genre)));
        }

        ViewBag.NumberOfResults = gamesInLibraryWithoutCollection.Count();
        // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
        var listWithoutCollection = await PaginatedList<Game>.CreateAsync(gamesInLibraryWithoutCollection.AsNoTracking(), pageNumber ?? 1, _pageSize);
        var dataWithoutCollection = new GameViewData
        {
          NumberOfResults = gamesInLibraryWithoutCollection.Count(),
          Games = listWithoutCollection,
          Genres = context.Genres.Distinct(),
          Platforms = context.Platforms.Distinct(),
        };
        return View(dataWithoutCollection);
      }


      if( !Enum.TryParse(collection, out GameStatus statusEnum) )
      {
        // Se a conversão falhar, retorne BadRequest
        ViewBag.Error = "Invalid status value.";
        ViewBag.Collection = "";
        ViewBag.NumberOfResults = "error";
        return RedirectToAction("UserLibrary", "Library", new { id = user.UserName });
      }

      var gamesInLibrary1 = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == user)
            .SelectMany(gl => gl.GameLogs.Where(g => g.Status == statusEnum)
            .Select(g => g.Game));

      if( !string.IsNullOrEmpty(releasePlatform) )
      {


        gamesInLibrary1 = gamesInLibrary1.Where(g => g.GamePlatforms.Any(gp => gp.Platform.PlatformName.Equals(releasePlatform)));
      }

      if( !string.IsNullOrEmpty(genre) )
      {

        gamesInLibrary1 = gamesInLibrary1.Where(g => g.GameGenres.Any(gg => gg.Genre.GenreName.Equals(genre)));
      }

      ViewBag.NumberOfResults = gamesInLibrary1.Count();


      var list = await PaginatedList<Game>.CreateAsync(gamesInLibrary1.AsNoTracking(), pageNumber ?? 1, _pageSize);
      var data = new GameViewData
      {
        NumberOfResults = gamesInLibrary1.Count(), // arranjar iste
        Games = list,
        Genres = context.Genres.Distinct(),
        Platforms = context.Platforms.Distinct(),
      };

      return View(data);
    }

    /// <summary>
    /// Action method for adding or updating games.
    /// </summary>
    /// <param name="gameId">The ID of the game to add or update.</param>
    /// <param name="status">The status of the game (e.g., completed, in-progress, etc.).</param>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddUpdateGames(long gameId, string status, string userId)
    {
      var user = context.Users.Where(u => u.Id == userId).First();
      var game = context.Games.Where(g => g.IgdbId == gameId).First();

      if( !Enum.TryParse(status, out GameStatus statusEnum) )
      {
        // Se a conversão falhar, retorne BadRequest
        ViewBag.Error = "Invalid status value.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      if( user == null || gameId <= 0 )
      {
        ViewBag.Error = "Invalid user or game ID.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      var library = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .FirstOrDefaultAsync(g => g.User == user);

      if( library == null )
      {
        library = new GamesLibrary
        {
          User = user,
          GameLogs = []
        };
        context.GamesLibrary.Add(library);
      }

      var existingGame = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
      if( existingGame != null )
      {
        // Atualizar o jogo existente
        existingGame.Status = statusEnum;
      }
      else
      {
        // Adicionar novo jogo à biblioteca do utilizador
        existingGame = new GameLog
        {
          Game = game,
          IgdbId = game.IgdbId,
          Status = statusEnum,
          UserId = user.Id,
          User = user
        };

        library.GameLogs.Add(existingGame);
      }

      await context.SaveChangesAsync();
      TempData["StatusMessage"] = $"{existingGame.Game.Name} log status updated to {statusEnum}.";
      return RedirectToAction("Details", "Games", new { id = game.IgdbId });
    }

    /// <summary>
    /// Action method for removing a game.
    /// </summary>
    /// <param name="gameId">The ID of the game to remove.</param>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveGame(long gameId)
    {
      var user = await signInManager.UserManager.GetUserAsync(User);
      var game = context.Games.Where(g => g.IgdbId == gameId).First();

      if( user == null || gameId <= 0 )
      {
        return BadRequest();
      }

      var library = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .Include(g => g.Top5Games)
          .FirstOrDefaultAsync(g => g.User == user);

      if( library == null )
      {
        return NotFound();
      }

      var gameToRemove = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
      if( gameToRemove != null )
      {
        if( library.Top5Games != null && library.Top5Games.Contains(gameToRemove) ) library.Top5Games.Remove(gameToRemove);
        context.GameLog.Remove(gameToRemove);
        await context.SaveChangesAsync();
      }
      TempData["StatusMessage"] = $"{gameToRemove.Game.Name} log was removed from your library.";
      return RedirectToAction("Details", "Games", new { id = game.IgdbId });
    }

    /// <summary>
    /// Action method for adding a review for a game.
    /// </summary>
    /// <param name="gameId">The ID of the game for which the review is being added.</param>
    /// <param name="reviewV">The review text.</param>
    /// <param name="ratingV">The rating given for the game.</param>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddReview(long gameId, string reviewV, int ratingV)
    {
      var user = await signInManager.UserManager.GetUserAsync(User);
      var game = context.Games.Where(g => g.IgdbId == gameId).First();

      if( user == null )
      {
        return BadRequest("User not authenticated.");
      }

      // Verifique se o jogo está na biblioteca do usuário
      var userLibrary = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .FirstOrDefaultAsync(g => g.User == user && g.GameLogs.Any(gl => gl.IgdbId == game.IgdbId));

      if( userLibrary == null )
      {
        // Adicione a mensagem de erro ao ViewBag
        return BadRequest("Game not found in library.");
      }

      // Encontre o GameLog correspondente
      var gameLog = userLibrary.GameLogs.FirstOrDefault(gl => gl.IgdbId == game.IgdbId);


      // Verifique se o GameLog existe e se os status estão preenchidos
      if( gameLog == null )
      {
        // Adicione a mensagem de erro ao ViewBag
        return BadRequest("Game status not filled.");
      }

      // Processe a avaliação e a revisão do jogo aqui
      // Atualize os campos de revisão e avaliação no GameLog
      gameLog.Review = reviewV;
      gameLog.Rating = ratingV;

      // Salve as alterações no banco de dados
      await context.SaveChangesAsync();

      // Redirecione de volta para a página de detalhes do jogo após a submissão
      TempData["StatusMessage"] = $"{gameLog.Game.Name} review and rating updated.";
      return RedirectToAction("Details", "Games", new { id = game.IgdbId });
    }

    /// <summary>
    /// Adds a game to the user's top 5 list.
    /// </summary>
    /// <param name="gameId">The ID of the game to add to the top 5 list.</param>
    /// <returns>An asynchronous task that represents the operation and returns an IActionResult.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToTop5(long gameId)
    {
      var user = await signInManager.UserManager.GetUserAsync(User);
      if( user == null ) return BadRequest();

      var library = await context.GamesLibrary.Include(l => l.Top5Games).Include(l => l.GameLogs).ThenInclude(gl => gl.Game)
        .FirstOrDefaultAsync(l => l.UserId == user.Id);
      if( library == null ) return BadRequest();

      var game = library.GameLogs.FirstOrDefault(g => g.IgdbId == gameId);
      if( game == null ) return BadRequest();

      library.Top5Games ??= [];

      if( library.Top5Games.Count >= 5 )
      {
        TempData["StatusMessage"] = "You can only add 5 games to the top 5 list. Remove one if you would like to add another.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }
      if( game.Game == null ) await Console.Out.WriteLineAsync("game is null");
      if( library.Top5Games.Contains(game) )
      {
        TempData["StatusMessage"] = $"{game.Game.Name} is already in your top 5 list.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }
      library.Top5Games.Add(game);
      await context.SaveChangesAsync();
      TempData["StatusMessage"] = $"{game.Game.Name} was added to your top 5 list.";
      return RedirectToAction("Details", "Games", new { id = gameId });
    }

    /// <summary>
    /// Removes a game from the user's top 5 list.
    /// </summary>
    /// <param name="gameId">The ID of the game to remove from the top 5 list.</param>
    /// <returns>An asynchronous task that represents the operation and returns an IActionResult.</returns>
    public async Task<IActionResult> RemoveFromTop5(long gameId)
    {
      var user = await signInManager.UserManager.GetUserAsync(User);
      if( user == null ) return BadRequest();

      var library = await context.GamesLibrary.Include(l => l.Top5Games).ThenInclude(gl => gl.Game)
          .FirstOrDefaultAsync(l => l.UserId == user.Id);
      if( library == null ) return BadRequest();

      var game = library.Top5Games.FirstOrDefault(g => g.IgdbId == gameId);
      if( game == null ) return BadRequest();

      library.Top5Games ??= [];

      if( game.Game == null ) await Console.Out.WriteLineAsync("game is null");

      if( !library.Top5Games.Contains(game) )
      {
        TempData["StatusMessage"] = $"{game.Game.Name} is not in your top 5 list.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      library.Top5Games.Remove(game);
      await context.SaveChangesAsync();
      TempData["StatusMessage"] = $"{game.Game.Name} was removed from your top 5 list.";
      return RedirectToAction("Details", "Games", new { id = gameId });
    }

    /// <summary>
    /// Retrieves the top 5 game logs for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user for whom to retrieve the game logs.</param>
    /// <param name="context">The application database context.</param>
    /// <returns>A list of the top 5 game logs for the specified user.</returns>
    public static async Task<List<GameLog>> GetTop5(string userId, ApplicationDbContext context)
    {
      var library = await context.GamesLibrary.
        Include(l => l.Top5Games).ThenInclude(gl => gl.Game).
        FirstOrDefaultAsync(l => l.UserId == userId);
      return library == null ? [] : library.Top5Games;
    }

    [Authorize]
    [HttpPost]
    [Route("ImportLibrary")]
    /// <summary>
    /// Imports the game library from Steam for the specified Steam ID.
    /// </summary>
    /// <param name="steamId">The Steam ID of the user.</param>
    /// <returns>A redirection to the user's library view.</returns>
    public async Task<IActionResult> ImportLibrary(string steamId)
    {
      List<SteamAPI.GameInfo> gamesInfoInSteamLibrary = [];
      List<GameLog> finalGameLogs = [];
      foreach( SteamAPI.GameInfo gameInfo in await steamAPI.GetUserLibrary(steamId) )
      {
        Game? game = await context.Games.FirstOrDefaultAsync(g => g.Name != null && g.Name.Equals(gameInfo.Name));
        if( game != null )
        {
          finalGameLogs.Add(new()
          {
            Game = game,
            GameId = game.GameId,
            IgdbId = game.IgdbId,
            HoursPlayed = gameInfo?.Playtime ?? 0,
            Status = gameInfo?.Playtime > 0 ? GameStatus.Playing : GameStatus.Backlogged
          });
        }
        else gamesInfoInSteamLibrary.Add(gameInfo);
      }

      finalGameLogs.AddRange(await steamAPI.GetGamesFromIGDB(gamesInfoInSteamLibrary));

      // udpating/adding games in db
      {
        var games = finalGameLogs.Select(g => g.Game).Where(g => g != null).Cast<Game>();

        var companyIds = games.SelectMany (g => g.GameCompanies != null ? g.GameCompanies.Where(c=>c!=null).Select(c => c.IgdbCompanyId) : []).Distinct().ToList();
        var platformIds = games.SelectMany(g => g.GamePlatforms!=null ? g.GamePlatforms.Select(p => p.IgdbPlatformId) : []).Distinct().ToList();
        var genresIds = games.SelectMany(g => g.GameGenres!=null ? g.GameGenres.Select(gg => gg.IgdbGenreId) : []).Distinct().ToList();

        var existingCompanies = await context.Companies.Where(c => companyIds.Contains(c.IgdbCompanyId)).ToListAsync();
        var existingPlatforms = await context.Platforms.Where(p => platformIds.Contains(p.IgdbPlatformId)).ToListAsync();
        var existingGenres = await context.Genres.Where(g => genresIds.Contains(g.IgdbGenreId)).ToListAsync();

        bool allCompaniesExist = existingCompanies.Count == companyIds.Count;
        bool allPlatformsExist = existingPlatforms.Count == platformIds.Count;
        bool allGenresExist = existingGenres.Count == genresIds.Count;

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allCompaniesExist )
            {

              var missingCompanies = await igdbService.GetCompaniesFromIds(
                  companyIds.Except(existingCompanies.Select(c => c.IgdbCompanyId)).ToList());

              foreach( var company in missingCompanies )
              {
                var existingCompany = await context.Companies.FirstOrDefaultAsync(g => g.IgdbCompanyId == company.IgdbCompanyId);
                if( existingCompany == null )
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
          catch( Exception ex )
          {
            await transaction.RollbackAsync();
            return RedirectToAction("Error", "Home");
          }

        }

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allPlatformsExist )
            {
              var missingPlatforms = await igdbService.GetPlatformsFromIds(
                  platformIds.Except(existingPlatforms.Select(c => c.IgdbPlatformId)).ToList());

              foreach( var platform in missingPlatforms )
              {
                var existingPlatform = await context.Platforms.FirstOrDefaultAsync(g => g.IgdbPlatformId == platform.IgdbPlatformId);
                if( existingPlatform == null )
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
          catch( Exception ex )
          {
            await Console.Out.WriteLineAsync(ex.Message);
            // Rollback da transação em caso de erro
            await transaction.RollbackAsync();
            // Tratar ou registrar o erro conforme necessário
            return RedirectToAction("Error", "Home");
          }
        }

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allGenresExist )
            {
              var missingGenres = await igdbService.GetGenresFromIds(
                  genresIds.Except(existingGenres.Select(gg => gg.IgdbGenreId)).ToList());

              foreach( var genre in missingGenres )
              {
                var existingGenre = await context.Genres.FirstOrDefaultAsync(g => g.IgdbGenreId == genre.IgdbGenreId);
                if( existingGenre == null )
                {
                  var newGenre = new Genre
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
            await transaction.CommitAsync();
          }
          catch( Exception ex )
          {
            await transaction.RollbackAsync();
            return RedirectToAction("Error", "Home");
          }
        }

        foreach( var game in games )
        {
          var existingGame = await context.Games.FirstOrDefaultAsync(g => g.IgdbId == game.IgdbId);

          if( existingGame == null )
          {
            var newGame = new Game
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
            foreach( var company in existingCompanies )
            {
              var companyBelongsToGame = game.GameCompanies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);

              if( companyBelongsToGame != null )
              {
                context.GameCompany.Add(new GameCompany
                {
                  Game = newGame,
                  Company = company
                });
              }
            }

            foreach( var genre in existingGenres )
            {
              var existingGenre = game.GameGenres.FirstOrDefault(g => g.IgdbGenreId == genre.IgdbGenreId);
              if( existingGenre != null )
              {
                context.GameGenre.Add(new GameGenre
                {
                  Game = newGame,
                  Genre = genre
                });
              }
            }

            foreach( var platform in existingPlatforms )
            {
              var existingPlatform = game.GamePlatforms.FirstOrDefault(p => p.IgdbPlatformId == platform.IgdbPlatformId);
              if( existingPlatform != null )
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

        await SaveChangesAsync(context);
      }

      var user = await signInManager.UserManager.GetUserAsync(User);
      if( user == null ) return BadRequest();
      user.SteamID = steamId;
      await SaveChangesAsync(context);
      var library = await context.GamesLibrary.Include(l=>l.GameLogs).FirstOrDefaultAsync(l => l.User == user);
      if( library == null )
      {
        library = new() { User = user, GameLogs = [], Top5Games = [] };
        context.GamesLibrary.Add(library);
        await SaveChangesAsync(context);
      }
      library.GameLogs ??= [];
      foreach( GameLog gl in finalGameLogs )
      {
        var existingGL = library.GameLogs.FirstOrDefault(g => g.IgdbId == gl.IgdbId);
        if( existingGL != null )
        {
          existingGL.Status = gl.Status;
        }
        else
        {
          var game = await context.Games.FirstOrDefaultAsync(g=> g.IgdbId == gl.IgdbId);
          if( game != null )
          {
            gl.Game = game;
            library.GameLogs.Add(gl);
          }
        }
        await SaveChangesAsync(context);
      }
      return RedirectToAction("UserLibrary", "Library", new { id = signInManager.UserManager.GetUserName(User) });
    }

    private static async Task<int> SaveChangesAsync(ApplicationDbContext context)
    {
      try
      {
        return await context.SaveChangesAsync();
      }
      catch( Exception e )
      {
        if( e is DbUpdateConcurrencyException || e is DbUpdateException )
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
  }
}