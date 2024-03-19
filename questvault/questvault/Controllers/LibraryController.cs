using IGDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using IGDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using questvault.Services;
using RestEase;
using System.Threading.Tasks;
using GameStatus = questvault.Models.GameStatus;

namespace questvault.Controllers
{
    public class LibraryController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
    {


        [HttpPost]
        public async Task<IActionResult> AddUpdateGames(long gameId, string ownage, string status)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);
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
                    GameLogs = new List<GameLog>()
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
                existingGame = new GameLog();

                existingGame.Game = game;
                existingGame.IgdbId = game.IgdbId;
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

            var gameToRemove = library.GameLogs.FirstOrDefault(g => g.IgdbId == game.IgdbId);
            if (gameToRemove != null)
            {
                context.GameLog.Remove(gameToRemove);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Games", new { id = game.IgdbId });
        }

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


    }
}