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
    public class TesteIGDBController : Controller
    {
        private readonly ApplicationDbContext _context;



        public TesteIGDBController(ApplicationDbContext context)
        {
            _context = context;
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
                var _igdbService = new IGDBService("uzhx4rrftyohllg1mrpy3ajo7090q5", "7rvcth933kxra92ddery5qn3jxwap7");
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
                var _igdbService = new IGDBService("uzhx4rrftyohllg1mrpy3ajo7090q5", "7rvcth933kxra92ddery5qn3jxwap7");
                Console.WriteLine("term: " + searchTerm);
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
