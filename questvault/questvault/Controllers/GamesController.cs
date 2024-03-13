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
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceIGDB _igdbService;


        public GamesController(ApplicationDbContext context, IServiceIGDB igdbService)
        {
            _context = context;
            _igdbService = igdbService;
        }


        // GET: TesteIGDB
        public async Task<IActionResult> Index()
        {
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
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




    }
}
