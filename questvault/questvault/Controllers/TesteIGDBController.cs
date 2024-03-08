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

            }catch (ApiException ex) {
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

        // GET: TesteIGDB/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogos = await _context.Jogos
                .FirstOrDefaultAsync(m => m.JogoId == id);
            if (jogos == null)
            {
                return NotFound();
            }

            return View(jogos);
        }

        // GET: TesteIGDB/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TesteIGDB/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JogoId,Nome,Genero")] Jogos jogos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jogos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jogos);
        }

        // GET: TesteIGDB/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogos = await _context.Jogos.FindAsync(id);
            if (jogos == null)
            {
                return NotFound();
            }
            return View(jogos);
        }

        // POST: TesteIGDB/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JogoId,Nome,Genero")] Jogos jogos)
        {
            if (id != jogos.JogoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jogos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JogosExists(jogos.JogoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jogos);
        }

        // GET: TesteIGDB/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogos = await _context.Jogos
                .FirstOrDefaultAsync(m => m.JogoId == id);
            if (jogos == null)
            {
                return NotFound();
            }

            return View(jogos);
        }

        // POST: TesteIGDB/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jogos = await _context.Jogos.FindAsync(id);
            if (jogos != null)
            {
                _context.Jogos.Remove(jogos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JogosExists(int id)
        {
            return _context.Jogos.Any(e => e.JogoId == id);
        }
    }
}
