using Microsoft.AspNetCore.Mvc;
using questvault.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using questvault.Data;
using Microsoft.EntityFrameworkCore;

namespace questvault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult PublicProfile()
        {
            return View();
        }

        public IActionResult PrivateProfile()
        {
            return View();
        }

       

        public async Task<IActionResult> AllUsersTestAsync()
        {
            var dbContext = _context.Users;
            return View(await dbContext.ToListAsync());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
