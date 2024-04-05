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
      ///     Loads the necessary statistics from admin side
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
        var loginInstances = await context.LogginInstances.ToListAsync();
        
        var uniqueSortedDates = loginInstances
          .Select(l => l.LoginDate)
          .Distinct() // Remover datas duplicadas
          .OrderBy(date => date) // Ordenar as datas
          .ToList();

        var loginCountByDate = loginInstances
          .GroupBy(l => l.LoginDate)
          .Select(group => new { Date = group.Key, Count = group.Count() })
          .OrderBy(x => x.Date) // Ordena pelo dia para ter os dados em sequência
          .ToList();
        List<int> counts = loginCountByDate.Select(x => x.Count).ToList();

        List<string> dummyDates = new () {"03/04/2024", "04/04/2024", "05/04/2024", "06/04/2024"};
        List<int> dummyDatesCount = new () { 2, 5, 7, 5 };
        
        var data = new Statistics
        {
          registeredUsersCount = registeredAccounts.Count,
          dateList = dummyDates,
          dateCountList = dummyDatesCount
        };


        return View("StatisticsPage", data);
      }
    }
}
