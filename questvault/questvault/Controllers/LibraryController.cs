using Microsoft.AspNetCore.Mvc;
using questvault.Data;
using questvault.Models;
using questvault.Services;
using RestEase;

namespace questvault.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<GamesLibrary> Libraries { get; set; }

        public LibraryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        
    }
}
