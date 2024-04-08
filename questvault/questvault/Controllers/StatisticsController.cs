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
        var allGameLogsRating = await context.GameLog.Select(gl => gl.Rating).ToListAsync();
        if(!allGameLogsRating.Any()) { allGameLogsRating.Add(0); }
        var loginInstances = await context.LogginInstances.ToListAsync();
        var accessInstances = await context.AccessInstances.ToListAsync();

        var allGenres = context.GameLog
          .SelectMany(log => log.Game.GameGenres)
          .Where(genre => genre != null)
          .Select(genre => genre.Genre)
          .ToList();


        var top5GenreCounts = allGenres
          .GroupBy(genre => genre)
          .Select(group => new { Genre = group.Key, Count = group.Count() })
          .OrderByDescending(g => g.Count)
          .Take(5)
          .ToList();

        List<string> genreNames = new List<string>();
        List<int> genreCount = new List<int>();

        foreach (var c in top5GenreCounts)
        {
          genreNames.Add(c.Genre.GenreName);
          genreCount.Add(c.Count);
        }

      // Lista de datas ordenadas sem repetição de todos os logins
      var uniqueSortedLoginDates = loginInstances
            .Select(l => l.LoginDate)
            .Distinct() // Remover datas duplicadas
            .OrderByDescending(date => date).Take(5) //seleciona as ultimas 5
            .OrderBy(date => date) // Ordenar as datas
            .ToList();

        // Lista de datas ordenadas sem repetição de todos os acessos
        var uniqueSortedAccessDates = accessInstances
          .Select(l => l.AccessDate)
          .Distinct() // Remover datas duplicadas
          .OrderByDescending(date => date).Take(5) //seleciona as ultimas 5
          .OrderBy(date => date) // Ordenar as datas
          .ToList();

        // Lista das ocurrencias de cada data na lista anterior
        var loginCountByDate = loginInstances
          .GroupBy(l => l.LoginDate)
          .Select(group => new { Date = group.Key, Count = group.Count() })
          .OrderByDescending(x => x.Date).Take(5) //seleciona os ultimos 5
          .OrderBy(x => x.Date) //Ordena por ordem crescente
          .ToList();

        // Lista das ocurrencias de cada data na lista anterior
        var accessCountByDate = accessInstances
          .GroupBy(l => l.AccessDate)
          .Select(group => new { Date = group.Key, Count = group.Count() })
          .OrderByDescending(x => x.Date).Take(5) //seleciona os ultimos 5
          .OrderBy(x => x.Date) //Ordena por ordem crescente
          .ToList();

        // Lista auxiliar para tranformar DateOnly em string para os Logins
        List<int> countsLogins = loginCountByDate.Select(x => x.Count).ToList();
        List<string> uniqueSortedLoginDatesCopy = new List<string>();
        foreach(var date in uniqueSortedLoginDates) { uniqueSortedLoginDatesCopy.Add(date.ToString()); }

        // Lista auxiliar para tranformar DateOnly em string para os Acessos
        List<int> countsAccess = accessCountByDate.Select(x => x.Count).ToList();
        List<string> uniqueSortedAccessDatesCopy = new List<string>();
        foreach(var date in uniqueSortedAccessDates) { uniqueSortedAccessDatesCopy.Add(date.ToString()); }

        var data = new Statistics
        {
          registeredUsersCount = registeredAccounts.Count,
          gameRatingAverage = Math.Round((double)allGameLogsRating.Average(), 2),
          LoginDateList = uniqueSortedLoginDatesCopy,
          LoginDateCountList = countsLogins,
          AccessDateCountList = countsAccess,
          AccessDateList = uniqueSortedAccessDatesCopy,
          GenreNames = genreNames,
          GenreCount = genreCount
        };


        return View("StatisticsPage", data);
      }
    }
}
