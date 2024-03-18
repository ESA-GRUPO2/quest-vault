using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;
using RestEase;

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
                return BadRequest("Invalid ownage or status value.");
            }

            if (user == null || gameId == null)
            {
                return BadRequest();
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

            var gameToRemove = library.GameLogs.FirstOrDefault(g => g.GameId == gameId);
            if (gameToRemove != null)
            {
                library.GameLogs.Remove(gameToRemove);
                await context.SaveChangesAsync();
            }

            return View();
        }

        public async Task<IActionResult> GameDetails(long gameId)
        {
            // Verifique se o jogo está na biblioteca do utilizador
            var user = await signInManager.UserManager.GetUserAsync(User);

            var userLibrary = await context.GamesLibrary
                .Include(g => g.GameLogs)
                .FirstOrDefaultAsync(g => g.User == user);

            bool isGameAddedToLibrary = userLibrary != null && userLibrary.GameLogs.Any(g => g.GameId == gameId);

            // Passe a variável para a visualização
            ViewBag.IsGameAddedToLibrary = isGameAddedToLibrary;

            // Obtenha os detalhes do jogo e retorne a visualização
            var game = await context.Games.FindAsync(gameId);
            return View(game);
        }
    }
}
