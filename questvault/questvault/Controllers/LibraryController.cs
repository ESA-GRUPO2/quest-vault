using IGDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;
using RestEase;
using GameStatus = questvault.Models.GameStatus;

namespace questvault.Controllers
{
    public class LibraryController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
    {
      

        [HttpPost]
        public async Task<IActionResult> AddUpdateGames(long gameId ,string ownage, string status)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);
            var game = context.Games.Where(g => g.IgdbId == gameId).First();

            if (!Enum.TryParse<OwnageStatus>(ownage, out OwnageStatus ownageEnum) ||
                !Enum.TryParse<GameStatus>(status, out GameStatus statusEnum))
            {
                // Se a conversão falhar, retorne BadRequest
                ViewBag.Error = "Invalid ownage or status value.";
                return RedirectToAction("Details", "Games", new { id = gameId });
            }

            if (user == null || gameId == null)
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
                    GameLogs = new List<GameLog>()
                };
                context.GamesLibrary.Add(library);
            }

            var existingGame = library.GameLogs.FirstOrDefault(g => g.GameId == game.IgdbId);
            if (existingGame != null)
            {
                // Atualizar o jogo existente
                existingGame.Status = statusEnum;
                existingGame.Ownage = ownageEnum;
            }
            else
            {
                // Adicionar novo jogo à biblioteca do utilizador
                existingGame = new GameLog();

                existingGame.Game = game;
                existingGame.Status = statusEnum;
                existingGame.Ownage = ownageEnum;

                library.GameLogs.Add(existingGame);
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Details", "Games", new { id = game.IgdbId });
        }

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

            var gameToRemove = library.GameLogs.FirstOrDefault(g => g.GameId ==game.IgdbId);
            if (gameToRemove != null)
            {
                context.GameLog.Remove(gameToRemove);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Games", new { id = game.IgdbId });
        }

        [Route("details/{gameid}")]
        public async Task<IActionResult> details(long gameId)
        {
            
            // Verifique se o jogo está na biblioteca do utilizador
            var user = await signInManager.UserManager.GetUserAsync(User);
            var game = context.Games.Where(g => g.IgdbId == gameId).First();

            var userLibrary = await context.GamesLibrary
                .Include(g => g.GameLogs)
                .FirstOrDefaultAsync(g => g.User == user);

            Console.WriteLine("ENTREI NESTA MERDA");

            bool isGameAddedToLibrary = userLibrary != null && userLibrary.GameLogs.Any(g => g.GameId == gameId);

            Console.WriteLine(isGameAddedToLibrary);
            // Passe a variável para a visualização
            ViewBag.IsGameAddedToLibrary = isGameAddedToLibrary;

            // Obtenha os detalhes do jogo e retorne a visualização
            return View(game);
        }

   

    }
}
