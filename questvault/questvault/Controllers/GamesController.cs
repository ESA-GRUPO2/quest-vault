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

        /// <summary>
        /// Constructor for GamesController.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        /// <param name="igdbService">The IGDB service for game-related operations.</param>
        public GamesController(ApplicationDbContext context, IServiceIGDB igdbService)
        {
            _context = context;
            _igdbService = igdbService;
        }


        /// <summary>
        /// GET action method for the Index view.
        /// </summary>
        /// <returns>Returns the Index view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Games == null) { return NotFound(); }

            var games = await _context.Games.ToListAsync();
            return View(games);
        }

        [HttpPost]
        [Route ("results")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Results(string searchTerm)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            if (searchTerm == null)
            {
                Console.WriteLine("primeiro if");
                return NotFound();
            }

            var games = await _igdbService.SearchGames(searchTerm);
            if (games == null)
            {
                Console.WriteLine("segundo if");
                return NotFound();
            }
            foreach (var game in games)
            {
                // Verifique se o jogo já existe na base de dados com base no GameId
                var existingGame = _context.Games.FirstOrDefault(g => g.IgdbId == game.IgdbId);
                await Console.Out.WriteLineAsync("exist?:-----> " + existingGame);
                // Se o jogo não existir, adicione-o à base de dados
                if (existingGame == null)
                {
                    await Console.Out.WriteLineAsync("tem jogo!");
                    // Crie um novo objeto Game com as propriedades desejadas
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
                        ReleaseDate = game.ReleaseDate
                    };
                    Console.WriteLine("ad: "+  newGame.ToString());
                    // Adicione o novo jogo à base de dados
                    _context.Games.Add(newGame);
                }
            }
            // Salve as mudanças na base de dados
            await _context.SaveChangesAsync();
            //var allgames = await _context.Games
            //    .Where(g => EF.Functions.Like(g.Name, $"%{searchInput}%")).ToListAsync();
            //return View(allgames);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
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

            return View(game);
        }


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
