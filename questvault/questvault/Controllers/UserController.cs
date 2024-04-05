using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
  public class UserController(ApplicationDbContext context) : Controller
  {

    // Ação para exibir o perfil do usuário
    public async Task<IActionResult> Profile(string name1, string id2)
    {
      bool friends = false;

      if (id2 == null || name1 == null)
      {
        return NotFound();
      }

      Console.WriteLine(name1);
      Console.WriteLine(name1);
      Console.WriteLine(name1);
      Console.WriteLine(name1);

      var friendships = await context.Friendship.ToListAsync();
      var user2 = await context.Users.FirstOrDefaultAsync(u => u.Id == id2);
      var user1 = await context.Users.FirstOrDefaultAsync(u => u.UserName == name1);

      if (user1 == null || user2 == null)
      {
        return NotFound();
      }

      foreach (var friendship in friendships)
      {
        if (friendship.User1 == user1 && friendship.User2 == user2 ||
            friendship.User1 == user2 && friendship.User2 == user1)
        {
          friends = true;
        }
      }

      if (user1 == user2)
      {
        return RedirectToAction("PublicProfile", "User", new { id = id2 });
      }

      if (user2.IsPrivate && !friends)
      {
        return RedirectToAction("PrivateProfile", "User", new { id = id2, friends });
      }
      else
      {
        return RedirectToAction("PublicProfile", "User", new { id = id2 });
      }
    }

    public async Task<IActionResult> PublicProfile(string id, bool friends)
    {
      if (id == null)
      {
        return NotFound();
      }
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      var friendsViewData = new FriendsViewData
      {
        user = user,
        friends = friends
      };

      ViewData["Top5"] = await LibraryController.GetTop5(id, context);
      return View(friendsViewData);
    }

    public async Task<IActionResult> PrivateProfile(string id)
    {
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }


    public async Task<bool> IsFriend(string username1, string username2)
    {
      bool friends = false;
      var user1 = await context.Users.FirstOrDefaultAsync(u => u.UserName == username1);
      var user2 = await context.Users.FirstOrDefaultAsync(u => u.UserName == username2);

      if (user1 == null || user2 == null)
      {
        return false;
      }
      var friendships = await context.Friendship.ToListAsync();

      foreach (var friendship in friendships)
      {
        if (friendship.User1 == user1 && friendship.User2 == user2 ||
            friendship.User1 == user2 && friendship.User2 == user1)
        {
          friends = true;
        }
      }
      return friends;
    }

  }
}

