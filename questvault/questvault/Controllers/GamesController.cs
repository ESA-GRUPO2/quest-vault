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
            var games = await _context.Games.ToListAsync();
            return View(games);
        }
        [HttpGet]
        [Route("games/details/{id}")]
        public IActionResult GameDetails(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// POST action method for handling the search form.
        /// </summary>
        /// <param name="searchTerm">The search term provided by the user.</param>
        /// <returns>Returns JSON data containing the search results.</returns>
        public async Task<IActionResult> SearchForm(string searchTerm)
        {
            try
            {
                var jogos = await _igdbService.SearchGames(searchTerm);
                Console.WriteLine("term: " + searchTerm);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([FromBody] string searchTerm)
        {
            try
            {
                var jogos = await _igdbService.SearchGames(searchTerm);
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

        [HttpGet ("Teste")]
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
