using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
    public class StatisticsController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
    {
        /// <summary>
        ///     Makes a list with all the user's friends
        /// </summary>
        [ActionName("StatisticsPage")]
        public async Task<IActionResult> StatisticsPageAsync()
        {
            var user = await signInManager.UserManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Redirect("/Identity/Account/Login");
            }
            var registeredAccounts = await context.Users.ToListAsync();

            return View("StatisticsPage", registeredAccounts.Count);
        }
    }
}
