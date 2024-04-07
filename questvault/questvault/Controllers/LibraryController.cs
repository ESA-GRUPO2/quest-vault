using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GameStatus = questvault.Models.GameStatus;
namespace questvault.Controllers
{
  public class LibraryController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
  {
    private readonly int _pageSize = 20;
    /// <summary>
    /// Action method for displaying the user's library.
    /// </summary>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpGet]
    [Route("UserLibrary")]
    public async Task<IActionResult> UserLibrary(string id, int? pageNumber, string? collection)
    {
      
      if (id == null)
      {
        ViewBag.Error = "Invalid user or game ID.";
        return NotFound();
      }

      var user = await context.Users.Where(u => u.NormalizedUserName == id.ToUpper()).FirstAsync();

      if (user == null)
      {
        ViewBag.Error = "Invalid user or game ID.";
        return NotFound();
      }

      if (String.IsNullOrEmpty(collection))
      {
        var gamesInLibraryWithoutCollection = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == user)
        .SelectMany(gl => gl.GameLogs.Select(g => g.Game))
        ;

        ViewBag.NumberOfResults = gamesInLibraryWithoutCollection.Count();
        // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
        var listWithoutCollection = await PaginatedList<Game>.CreateAsync(gamesInLibraryWithoutCollection.AsNoTracking(), pageNumber ?? 1, _pageSize);
        var dataWithoutCollection = new GameViewData
        {
          NumberOfResults = listWithoutCollection.Count,
          Games = listWithoutCollection
        };
        ViewBag.Collection = collection;
        return View(dataWithoutCollection);
      }


      if (!Enum.TryParse(collection, out GameStatus statusEnum))
      {
        // Se a conversão falhar, retorne BadRequest
        ViewBag.Error = "Invalid status value.";
        ViewBag.Collection = "";
        ViewBag.NumberOfResults = "error";
        return RedirectToAction("UserLibrary", "Library", new { id = user.UserName });
      }

      //var gamesInLibrary = context.GamesLibrary
      //        .Include(gl => gl.GameLogs) 
      //        .ThenInclude(gl => gl.Game) 
      //        .Where(gl => gl.User == user)
      //            .SelectMany(gl => gl.GameLogs.Where(g => g.Status == statusEnum)
      //            .Select(g => g.Game));

      var gamesInLibrary1 = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == user)
            .SelectMany(gl => gl.GameLogs.Where(g => g.Status == statusEnum)
            .Select(g => g.Game));
      ViewBag.NumberOfResults = gamesInLibrary1.Count();

      // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
      var list = await PaginatedList<Game>.CreateAsync(gamesInLibrary1.AsNoTracking(), pageNumber ?? 1, _pageSize);
      var data = new GameViewData
      {
        NumberOfResults = list.Count,
        Games = list
      };
      ViewBag.Collection = collection;
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

      if (!Enum.TryParse(status, out GameStatus statusEnum))
      {
        // Se a conversão falhar, retorne BadRequest
        ViewBag.Error = "Invalid status value.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      if (user == null || gameId <= 0)
      {
        ViewBag.Error = "Invalid user or game ID.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      var library = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .FirstOrDefaultAsync(g => g.User == user);

      if (library == null)
      {
        library = new GamesLibrary
        {
          User = user,
          GameLogs = []
        };
        context.GamesLibrary.Add(library);
      }

      var existingGame = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
      if (existingGame != null)
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

      if (user == null || gameId <= 0)
      {
        return BadRequest();
      }

      var library = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .Include(g => g.Top5Games)
          .FirstOrDefaultAsync(g => g.User == user);

      if (library == null)
      {
        return NotFound();
      }

      var gameToRemove = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
      if (gameToRemove != null)
      {
        if (library.Top5Games != null && library.Top5Games.Contains(gameToRemove)) library.Top5Games.Remove(gameToRemove);
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

      if (user == null)
      {
        return BadRequest("User not authenticated.");
      }

      // Verifique se o jogo está na biblioteca do usuário
      var userLibrary = await context.GamesLibrary
          .Include(g => g.GameLogs)
          .FirstOrDefaultAsync(g => g.User == user && g.GameLogs.Any(gl => gl.IgdbId == game.IgdbId));

      if (userLibrary == null)
      {
        // Adicione a mensagem de erro ao ViewBag
        return BadRequest("Game not found in library.");
      }

      // Encontre o GameLog correspondente
      var gameLog = userLibrary.GameLogs.FirstOrDefault(gl => gl.IgdbId == game.IgdbId);


      // Verifique se o GameLog existe e se os status estão preenchidos
      if (gameLog == null)
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
      if (user == null) return BadRequest();

      var library = await context.GamesLibrary.Include(l => l.Top5Games).Include(l => l.GameLogs).ThenInclude(gl => gl.Game)
        .FirstOrDefaultAsync(l => l.UserId == user.Id);
      if (library == null) return BadRequest();

      var game = library.GameLogs.FirstOrDefault(g => g.IgdbId == gameId);
      if (game == null) return BadRequest();

      library.Top5Games ??= [];

      if (library.Top5Games.Count >= 5)
      {
        TempData["StatusMessage"] = "You can only add 5 games to the top 5 list. Remove one if you would like to add another.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }
      if (game.Game == null) await Console.Out.WriteLineAsync("game is null");
      if (library.Top5Games.Contains(game))
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
      if (user == null) return BadRequest();

      var library = await context.GamesLibrary.Include(l => l.Top5Games).ThenInclude(gl => gl.Game)
          .FirstOrDefaultAsync(l => l.UserId == user.Id);
      if (library == null) return BadRequest();

      var game = library.Top5Games.FirstOrDefault(g => g.IgdbId == gameId);
      if (game == null) return BadRequest();

      library.Top5Games ??= [];

      if (game.Game == null) await Console.Out.WriteLineAsync("game is null");

      if (!library.Top5Games.Contains(game))
      {
        TempData["StatusMessage"] = $"{game.Game.Name} is not in your top 5 list.";
        return RedirectToAction("Details", "Games", new { id = gameId });
      }

      library.Top5Games.Remove(game);
      await context.SaveChangesAsync();
      TempData["StatusMessage"] = $"{game.Game.Name} was removed from your top 5 list.";
      return RedirectToAction("Details", "Games", new { id = gameId });
    }

    public static async Task<List<GameLog>> GetTop5(string userId, ApplicationDbContext context)
    {
      var library = await context.GamesLibrary.
        Include(l => l.Top5Games).ThenInclude(gl => gl.Game).
        FirstOrDefaultAsync(l => l.UserId == userId);
      return library == null ? [] : library.Top5Games;
    }
  }
}