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
using questvault.Migrations;
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
        public async Task<IActionResult> Index(string searchTerm)
        {
            var igdb = new IGDBClient("uzhx4rrftyohllg1mrpy3ajo7090q5", "7rvcth933kxra92ddery5qn3jxwap7");
            string query = $"fields id,name, genres; search *\"{searchTerm}\"*; limit 5;";
            try
            {
                var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query);
                Console.Write("TERMO: " + searchTerm);
                var jogos = games.Select(game => new Jogos
                {
                    JogoId = (int)game.Id,
                    Nome = game.Name,
                    Genero = ""
                }).ToList();


                //if (ModelState.IsValid)
                //{
                //    _context.Add(jogos);
                //    await _context.SaveChangesAsync();
                //    return RedirectToAction(nameof(Index));
                //}

                ViewBag.Titulo = "Detalhes do Jogo";
                ViewBag.Jogos = jogos;
                return View(jogos);

            }
            catch (ApiException ex)
            {
                // A ApiException contém detalhes sobre o erro, incluindo o código de status HTTP
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Manipule o BadRequest conforme necessário, por exemplo, obtenha a mensagem de erro
                    string mensagemErro = ex.Content;
                    // Faça algo com a mensagem de erro...
                    Console.WriteLine(mensagemErro);
                }
                // Trate outros tipos de exceções ou faça o necessário em caso de erro
                return View(); // Pode redirecionar para uma página de erro personalizada, por exemplo
            }
            catch (Exception ex)
            {
                // Trate outras exceções
                Console.WriteLine(ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchForm(string searchTerm)
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
