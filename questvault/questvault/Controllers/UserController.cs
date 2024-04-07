using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using System.Collections.Generic;

namespace questvault.Controllers
{
  public class UserController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
  {

    // Ação para exibir o perfil do usuário
    public async Task<IActionResult> Profile(string name1, string id2)
    {
      bool friends = false;
      bool send= false;
      bool received=false;

      if (id2 == null || name1 == null)
      {
        return NotFound();
      }

      var friendships = await context.Friendship.ToListAsync();
      var user2 = await context.Users.FirstOrDefaultAsync(u => u.Id == id2);
      var user1 = await context.Users.FirstOrDefaultAsync(u => u.UserName == name1);
      var requests = await context.FriendshipRequest.ToListAsync();

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

      foreach(var request in requests)
      {
        if (request.SenderId == user1.Id && request.ReceiverId == user2.Id)
        {
            send = true;
        }
        else if (request.SenderId == user2.Id && request.ReceiverId == user1.Id)
        {
            received = true;
        }
      }


      if (user1 == user2)
      {
        return RedirectToAction("PublicProfile", "User", new { id = id2});
      }

      if (user2.IsPrivate && !friends && user1.Clearance == 0)
      {
        return RedirectToAction("PrivateProfile", "User", new { id = id2, friends, send, received });
      }
      else
      {
        return RedirectToAction("PublicProfile", "User", new { id = id2, friends, send, received});
      }
    }

    public async Task<IActionResult> PublicProfile(string id, bool friends, bool send, bool received)
    {
      if (id == null)
      {
        return NotFound();
      }
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
      var userGameLogs = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == user)
        .SelectMany(gl => gl.GameLogs)
        .ToList();

      var ratingCount = context.GameLog?
          .Where(gl => gl.Rating.HasValue)
          .Where(gl => gl.User == user)
          .GroupBy(gl => gl.Rating.Value)
          .Select(group => new { Rating = group.Key, Count = group.Count() }) // Seleciona a chave de agrupamento (Rating) e a contagem
          .ToList();

      var allRatingsCount = Enumerable.Range(1, 5) // Gera uma sequência de 1 a 5
          .Select(rating => new 
          {
            Rating = rating,
            Count = ratingCount.FirstOrDefault(rc => rc.Rating == rating)?.Count ?? 0
          }) // Obtém a contagem do rating específico, se não encontrado, usa 0
          .OrderBy(r => r.Rating) // Garante a ordem pelo rating
          .Select(r => r.Count) // Seleciona apenas a contagem
          .ToList();

      var friendsViewData = new FriendsViewData
      {
        user = user,
        friends = friends,
        RequestSent = send,
        RequestRecieved = received,
        nJogosTotal = userGameLogs.Count(),
        nJogosPlaying = userGameLogs.Where(gl => gl.Status == GameStatus.Playing).Count(),
        nJogosComplete = userGameLogs.Where(gl => gl.Status == GameStatus.Complete).Count(),
        nJogosRetired = userGameLogs.Where(gl => gl.Status == GameStatus.Retired).Count(),
        nJogosBacklogged = userGameLogs.Where(gl => gl.Status == GameStatus.Backlogged).Count(),
        nJogosAbandoned = userGameLogs.Where(gl => gl.Status == GameStatus.Abandoned).Count(),
        nJogosWishlist = userGameLogs.Where(gl => gl.Status == GameStatus.Wishlist).Count(),
        ratingCountList = allRatingsCount
      };

      ViewData["Top5"] = await LibraryController.GetTop5(id, context);
      return View(friendsViewData);
    }

    public async Task<IActionResult> PrivateProfile(string id, bool friends, bool send, bool received)
    {
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if (user == null)
      {
        return NotFound();
      }

      var friendsViewData = new FriendsViewData
      {
        user = user,
        friends = friends,
        RequestSent = send,
        RequestRecieved = received
      };

      return View(friendsViewData);
    }
    public async Task<IActionResult> SendFriendRequestAsync(string id)
    {
      await FriendshipsController.SendFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(signInManager.UserManager.GetUserName(User), id);
    }

    public async Task<IActionResult> AcceptFriendRequestAsync(string id)
    {
      await FriendshipsController.AcceptFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(signInManager.UserManager.GetUserName(User), id);
    }

    public async Task<IActionResult> RejectFriendRequestAsync(string id)
    {
      await FriendshipsController.RejectFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(signInManager.UserManager.GetUserName(User), id);
    }

    public async Task<IActionResult> RemoveFriendAsync(string id)
    {
      await FriendshipsController.RemoveFriendAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(signInManager.UserManager.GetUserName(User), id);
    }
  }
}


