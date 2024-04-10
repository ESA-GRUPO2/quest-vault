using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
  public class FriendshipsController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
  {

    public static async Task SendFriendRequestAsync(string senderId, string receiverId, ApplicationDbContext context)
    {

      if (senderId == null || receiverId == null || context == null)
      {
        return;
      }
      var receiver = await context.Users.FindAsync(receiverId);
      var sender = await context.Users.FindAsync(senderId);
      if (receiver != null && sender != null && receiver != sender)
      {
        var existingFriendship = await context.Friendship.Where(f => (f.User1.Id == receiver.Id && f.User2.Id == sender.Id) || (f.User1.Id == sender.Id && f.User2.Id == receiver.Id)).FirstOrDefaultAsync();
        var existingRequest = await context.FriendshipRequest.Where(fr => (fr.Receiver.Id == receiver.Id && fr.Sender.Id == sender.Id) || (fr.Receiver.Id == sender.Id && fr.Sender.Id == receiver.Id)).FirstOrDefaultAsync();
        if (existingFriendship == null && existingRequest == null)
        {
          var friendshipRequest = new FriendshipRequest
          {
            Receiver = receiver,
            Sender = sender,
            isAccepted = false,
            FriendshipDate = DateTime.Now
          };
          context.FriendshipRequest.Add(friendshipRequest);
          await context.SaveChangesAsync();
        }
      }
    }
    /// <summary>
    ///     Verifies if a connection between two users already exists and if it doesnt a friend request object is created.
    /// </summary>
    /// <param name="id">The id of the user who recieves the friend request</param>
    public async Task<IActionResult> SendFriendRequestAsync(string id)
    {
      await SendFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);

      return RedirectToRoute(new { controller = "Friendships", action = "FriendsPage" });
    }

    public static async Task AcceptFriendRequestAsync(string senderId, string receiverId, ApplicationDbContext context)
    {
      if(senderId==null || receiverId == null|| context==null)
      {
        return;
      }
      var receiver = await context.Users.FindAsync(receiverId);
      var sender = await context.Users.FindAsync(senderId);
      var friendship = new Friendship { };
      if (receiver != null && sender != null)
      {
        friendship.User1 = receiver;
        friendship.User2 = sender;
        context.Friendship.Add(friendship);
        var friendshipRequest = await context.FriendshipRequest.Where(fr => fr.SenderId == sender.Id && fr.ReceiverId == receiver.Id).ToListAsync();
        if (friendshipRequest.Any())
        {
          context.FriendshipRequest.Remove(friendshipRequest.First());
        }
        else
        {
          friendshipRequest = await context.FriendshipRequest.Where(fr => fr.SenderId == receiver.Id && fr.ReceiverId == sender.Id).ToListAsync();
          context.FriendshipRequest.Remove(friendshipRequest.First());
        }
        context.SaveChanges();
      }
    }
    /// <summary>
    ///     Creates a friendship object between the current user and a selected user.
    /// </summary>
    /// <param name="id">The id of the user who recieves the friendship</param>
    public async Task<IActionResult> AcceptFriendRequestAsync(string id)
    {
      await AcceptFriendRequestAsync(signInManager.UserManager.GetUserId(User),id, context);
      
      return RedirectToRoute(new { controller = "Friendships", action = "FriendRequests" });
    }

    public static async Task RejectFriendRequestAsync(string senderId, string receiverId, ApplicationDbContext context)
    {
      if (senderId == null || receiverId == null || context == null)
      {
        return;
      }
      var receiver = await context.Users.FindAsync(receiverId);
      var sender = await context.Users.FindAsync(senderId);
      if (receiver != null && sender != null)
      {
        var friendshipRequest = await context.FriendshipRequest.Where(fr => fr.SenderId == sender.Id && fr.ReceiverId == receiver.Id).ToListAsync();
        if (friendshipRequest.Any())
        {
          context.FriendshipRequest.Remove(friendshipRequest.First());
        }
        else
        {
          friendshipRequest = await context.FriendshipRequest.Where(fr => fr.SenderId == receiver.Id && fr.ReceiverId == sender.Id).ToListAsync();
          context.FriendshipRequest.Remove(friendshipRequest.First());
        }
        context.SaveChanges();
      }
    }

    /// <summary>
    ///     Deletes a specific object of the type friendRequest
    /// </summary>
    /// <param name="id">The id of the user who sent the friend request</param>
    public async Task<IActionResult> RejectFriendRequestAsync(string id)
    {
      await RejectFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);

      return RedirectToRoute(new { controller = "Friendships", action = "FriendRequests" });

    }

    /// <summary>
    ///     Makes a list with all the user's friends
    /// </summary>
    [ActionName("FriendsPage")]
    public async Task<IActionResult> FriendsPageAsync()
    {

      var user = await signInManager.UserManager.GetUserAsync(this.User);
      if (user == null)
      {
        return Redirect("/Identity/Account/Login");
      }

      var dbContext = await context.Friendship.ToListAsync();
      var dbContextCopy = new List<Friendship>();
      foreach (var a in dbContext)
      {
        if (a.User1 == user)
        {
          var friendship = new Friendship
          {
            User1 = user,
          };
          var user2 = await context.Users.FindAsync(a.User2Id);
          friendship.User2 = user2;
          dbContextCopy.Add(friendship);
        }
        else if (a.User2 == user)
        {
          var friendship = new Friendship
          {
            User1 = user,
          };
          var user2 = await context.Users.FindAsync(a.User1Id);
          friendship.User2 = user2;
          dbContextCopy.Add(friendship);
        }

      }
      return View("FriendsPage", dbContextCopy);
    }

    // GET: FriendshipRequests
    public async Task<IActionResult> FriendRequestsAsync()
    {
      var user = await signInManager.UserManager.GetUserAsync(this.User);
      if (user == null)
      {
        return Redirect("/Identity/Account/Login");
      }

      var dbContext = await context.FriendshipRequest.ToListAsync();
      var dbContextCopy = new List<FriendshipRequest>();
      foreach (var a in dbContext)
      {
        if (a.Receiver == user)
        {
          var sender = await context.Users.FindAsync(a.SenderId);
          a.Sender = sender;
          dbContextCopy.Add(a);
        }

      }
      return View(dbContextCopy);
    }

    public static async Task RemoveFriendAsync(string senderId, string receiverId, ApplicationDbContext context)
    {
      if (senderId == null || receiverId == null || context == null)
      {
        return;
      }
      var user2 = await context.Users.FindAsync(receiverId);
      var user1 = await context.Users.FindAsync(senderId);

      if (user1 != null && user2 != null)
      {
        var friendship = await context.Friendship.Where(f => f.User1Id == user1.Id && f.User2Id == user2.Id).ToListAsync();
        if (friendship.Any())
        {
          context.Friendship.Remove(friendship.First());
        }
        else
        {
          friendship = await context.Friendship.Where(f => f.User1Id == user2.Id && f.User2Id == user1.Id).ToListAsync();
          context.Friendship.Remove(friendship.First());
        }
        context.SaveChanges();
      }
    }

    /// <summary>
    ///     Deletes a user from the list of friends
    /// </summary>
    /// <param name="id">The id of the other user</param>
    public async Task<IActionResult> RemoveFriendAsync(string id)
    {
      await RemoveFriendAsync(signInManager.UserManager.GetUserId(User), id, context);

      return RedirectToRoute(new { controller = "Friendships", action = "FriendsPage" });
    }
  }
}
