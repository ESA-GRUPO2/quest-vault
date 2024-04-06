using Microsoft.AspNetCore.Mvc;
using questvault.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using questvault.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<IActionResult> IndexAsync()
        {
          var accessInstance = new AccessInstance() { AccessDate = DateOnly.FromDateTime(DateTime.Now) };
          _context.AccessInstances.Add(accessInstance);
          await _context.SaveChangesAsync();
          return View();
        }

        //public IActionResult Error()
        //{
        //    return View();
        //}

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
        [HttpGet]
        [Route("SearchUser/{id}")]
        public async Task<IActionResult> SearchUser(string? id)
        {
            ViewBag.SearchTerm = id;
            await Console.Out.WriteLineAsync("CONTROLER SEARHC");
            if (id == null)
            {
                await Console.Out.WriteLineAsync("oqqq");
                return NotFound();
            }

            var users = _context.Users.Where(u => u.UserName.Contains(id));
            //if (users.IsNullOrEmpty())
            //{
            //    await Console.Out.WriteLineAsync("ai ta null");
            //    return NotFound();
            //}
            ViewBag.NumberResults = users.Count();
            return View(await users.ToListAsync());
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
