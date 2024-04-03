using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using GameStatus = questvault.Models.GameStatus;

namespace questvault.Controllers
{
  public class LibraryController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
  {
    /// <summary>
    /// Action method for displaying the user's library.
    /// </summary>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpGet]
    [Route("UserLibrary")]
    public async Task<IActionResult> UserLibrary(string id)
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

      var gamesInLibrary = context.GamesLibrary
              .Include(gl => gl.GameLogs) // Inclua os GameLogs para evitar carregamento preguiçoso
              .ThenInclude(gl => gl.Game) // Inclua os jogos dentro de cada GameLog
              .Where(gl => gl.User == user) // Filtre pela biblioteca do usuário atual
              .SelectMany(gl => gl.GameLogs.Select(g => g.Game)) // Selecione todos os jogos dentro dos GameLogs
              .ToList();


      // Realize a pesquisa na base de dados pelo searchTerm e retorne os resultados para a view
      var data = new GameViewData
      {
        NumberOfResults = gamesInLibrary.Count,
        Games = gamesInLibrary
      };
      return View(data);
    }

    /// <summary>
    /// Action method for adding or updating games.
    /// </summary>
    /// <param name="gameId">The ID of the game to add or update.</param>
    /// <param name="ownage">The ownage status of the game (e.g., owned, wishlist, etc.).</param>
    /// <param name="status">The status of the game (e.g., completed, in-progress, etc.).</param>
    /// <returns>An asynchronous task representing the operation with IActionResult result.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddUpdateGames(long gameId, string ownage, string status, string userId)
    {
      var user = context.Users.Where(u => u.Id == userId).First();
      var game = context.Games.Where(g => g.IgdbId == gameId).First();

      if (!Enum.TryParse(ownage, out OwnageStatus ownageEnum) ||
          !Enum.TryParse(status, out GameStatus statusEnum))
      {
        // Se a conversão falhar, retorne BadRequest
        ViewBag.Error = "Invalid ownage or status value.";
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
        existingGame.Ownage = ownageEnum;
      }
      else
      {
        // Adicionar novo jogo à biblioteca do utilizador
        existingGame = new GameLog
        {
          Game = game,
          IgdbId = game.IgdbId,
          Status = statusEnum,
          Ownage = ownageEnum
        };

        library.GameLogs.Add(existingGame);
      }

      await context.SaveChangesAsync();

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
          .FirstOrDefaultAsync(g => g.User == user);

      if (library == null)
      {
        return NotFound();
      }

      var gameToRemove = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
      if (gameToRemove != null)
      {
        context.GameLog.Remove(gameToRemove);
        await context.SaveChangesAsync();
      }

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
      return RedirectToAction("Details", "Games", new { id = game.IgdbId });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddToTop5(long gameId)
    {
      var user = await signInManager.UserManager.GetUserAsync(User);
      if (user == null) return BadRequest();
      var library = await context.GamesLibrary.Include(l => l.GameLogs)
        .FirstOrDefaultAsync(l => l.UserId == user.Id);
      if (library == null) return BadRequest();

      var gameLog = library.GameLogs.FirstOrDefault(gl => gl.IgdbId == gameId);
      if (gameLog == null) return BadRequest();
      library.Top5Games ??= [];
      if (library.Top5Games.Count >= 5)
      {
        TempData["StatusMessage"] = "You can only add 5 games to the top 5 list. Remove one if you would like to add another.";
        return RedirectToAction("Details", "Games", new { id = gameLog.IgdbId });
      }
      library.Top5Games.Add(gameLog);
      TempData["StatusMessage"] = $"{gameLog.Game?.Name} was added to your top 5.";
      Console.WriteLine("library: " + library.Top5Games.Count);
      return RedirectToAction("Details", "Games", new { id = gameLog.IgdbId });
    }
  }
}