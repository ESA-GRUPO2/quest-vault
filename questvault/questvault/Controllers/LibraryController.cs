using Microsoft.AspNetCore.Mvc;
using questvault.Data;
using questvault.Services;

namespace questvault.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;

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
