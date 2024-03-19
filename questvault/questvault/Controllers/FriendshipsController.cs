using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
    public class FriendshipsController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
    {

        /// <summary>
        ///     Verifies if a connection between two users already exists and if it doesnt a friend request object is created.
        /// </summary>
        /// <param name="id">The id of the user who recieves the friend request</param>
        public async Task<IActionResult> SendFriendRequestAsync(string id)
        {
            var receiver = await context.Users.FindAsync(id);
            var sender = await signInManager.UserManager.GetUserAsync(this.User);
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
                else
                {
                    Console.Out.WriteLine("ALREADY EXISTS");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///     Creates a friendship object between the current user and a selected user.
        /// </summary>
        /// <param name="id">The id of the user who recieves the friendship</param>
        public async Task<IActionResult> AcceptFriendRequestAsync(string id)
        {
            var receiver = await context.Users.FindAsync(id);
            var sender = await signInManager.UserManager.GetUserAsync(this.User);
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
            return RedirectToRoute(new { controller = "Friendships", action = "FriendRequests" });
        }

        /// <summary>
        ///     Deletes a specific object of the type friendRequest
        /// </summary>
        /// <param name="id">The id of the user who sent the friend request</param>
        public async Task<IActionResult> RejectFriendRequestAsync(string id)
        {
            var sender = await context.Users.FindAsync(id);
            var receiver = await signInManager.UserManager.GetUserAsync(this.User);
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
            return RedirectToRoute(new { controller = "Friendships", action = "FriendRequests" });

        }

        /// <summary>
        ///     Makes a list with all the user's friends
        /// </summary>
        [ActionName("FriendsPage")]
        public async Task<IActionResult> FriendsPageAsync()
        {

            var user = await signInManager.UserManager.GetUserAsync(this.User);

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
                }else if(a.User2 == user)
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
            return View("FriendsPage",dbContextCopy);
        }

        // GET: FriendshipRequests
        public async Task<IActionResult> FriendRequestsAsync()
        {
            var user = await signInManager.UserManager.GetUserAsync(this.User);

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


        /// <summary>
        ///     Deletes a user from the list of friends
        /// </summary>
        /// <param name="id">The id of the other user</param>
        public async Task<IActionResult> RemoveFriendAsync(string id)
        {
            var user1 = await signInManager.UserManager.GetUserAsync(this.User);
            var user2 = await context.Users.FindAsync(id);
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

            return RedirectToRoute(new { controller = "Friendships", action = "FriendsPage" });

        }
    }
}
