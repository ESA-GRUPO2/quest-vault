using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<User> _signInManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<User> signInManager)
    {
      _logger = logger;
      _context = context;
      _signInManager = signInManager;
    }

    public async Task<IActionResult> IndexAsync()
    {
      var accessInstance = new AccessInstance() { AccessDate = DateOnly.FromDateTime(DateTime.Now) };
      _context.AccessInstances.Add(accessInstance);
      await _context.SaveChangesAsync();

      if( _signInManager.IsSignedIn(User) )
      {
        return RedirectToAction("UserLibrary", "Library", new { id = _signInManager.UserManager.GetUserName(User) });
      }

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


    public async Task<IActionResult> AllUsersAsync()
    {
      var dbContext = _context.Users;
      return View(await dbContext.ToListAsync());
    }
    [HttpGet]
    [Route("SearchUser/{id}")]
    public async Task<IActionResult> SearchUser(string? id)
    {
      ViewBag.SearchTerm = id;
      if( id == null )
      {
        return NotFound();
      }

      var users = _context.Users.Where(u => u.UserName!= null && u.UserName.Contains(id) && !u.LockoutEnabled);
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
