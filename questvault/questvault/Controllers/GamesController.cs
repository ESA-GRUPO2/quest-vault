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
                .FirstOrDefaultAsync(m => m.GameId == id);


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
                .Where(g => g.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(s => new GameViewModel
                {
                    GameId = s.GameId,
                    Name = s.Name
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

        [HttpGet("Teste")]
        public async Task<IActionResult> Popular()
        {
            try
            {
                var jogos = await _igdbService.GetPopularGames(10);
                // Retorna os dados como JSON
                return Json(new { Jogos = jogos });
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.Content);
                return Json(new { Erro = "Erro na API IGDB" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { Erro = "Erro interno no servidor" });
            }
        }



    }
}
