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

        // GET: FriendshipRequests
        public async Task<IActionResult> IndexAsync()
        {
            //if (id != null) PatientList = _context.Patient.Include(p => p.Doctor).AsNoTracking().Where(p => p.DoctorId == id);
            //else PatientList = _context.Patient.Include(p => p.Doctor).AsNoTracking().Select(p => p);
            //context.FriendshipRequest.Include(f => f.Sender).AsNoTracking().Where(f => f.SenderId == signInManager.UserManager.GetUserAsync(this.User).Id.ToString())
            var user = await signInManager.UserManager.GetUserAsync(this.User);

            var dbContext = await context.FriendshipRequest.ToListAsync();
            var dbContextCopy = new List<FriendshipRequest>();
            foreach(var a in dbContext)
            {
                if(a.Receiver == user)
                {
                    var sender = await context.Users.FindAsync(a.SenderId);
                    a.Sender = sender;
                    dbContextCopy.Add(a);
                }
                
            }
            //Console.WriteLine(dbContext);
            return View(dbContextCopy);
        }
        /*public Task<IActionResult> Index()
        {
            return _context.FriendshipRequest != null ?
             View(await _context.FriendshipRequest.ToListAsync()) :
             Problem("Entity set 'ApplicationDbContext.FriendRequest'  is null.");
        }*/

        // GET: Friendships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await context.Friendship
                .FirstOrDefaultAsync(m => m.id == id);
            if (friendship == null)
            {
                return NotFound();
            }

            return View(friendship);
        }

        // GET: Friendships/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Friendships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id")] Friendship friendship)
        {
            if (ModelState.IsValid)
            {
                context.Add(friendship);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(friendship);
        }

        // GET: Friendships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await context.Friendship.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }
            return View(friendship);
        }

        // POST: Friendships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id")] Friendship friendship)
        {
            if (id != friendship.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(friendship);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendshipExists(friendship.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(friendship);
        }

        // GET: Friendships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await context.Friendship
                .FirstOrDefaultAsync(m => m.id == id);
            if (friendship == null)
            {
                return NotFound();
            }

            return View(friendship);
        }

        // POST: Friendships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friendship = await context.Friendship.FindAsync(id);
            if (friendship != null)
            {
                context.Friendship.Remove(friendship);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }

        private bool FriendshipExists(int id)
        {
            return context.Friendship.Any(e => e.id == id);
        }

        /// <summary>
        ///     Verifies if a connection between two users already exists and if it doesnt a friend request object is created.
        /// </summary>
        /// <param name="id">The id of the user who recieves the friend request</param>
        public async Task<IActionResult> SendFriendRequestAsync(string id)
        {
            var receiver = await context.Users.FindAsync(id);
            var sender = await signInManager.UserManager.GetUserAsync(this.User);
            if (receiver != null && sender != null)
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
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///     Makes a list with all the user's friends
        /// </summary>
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
            //Console.WriteLine(dbContext);
            return View(dbContextCopy);
        }


        /// <summary>
        ///     Deletes a user from the list of friends
        /// </summary>
        /// <param name="id">The id of the user who recieves the friendship</param>
        public async Task<IActionResult> RemoveFriendAsync(string id)
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
            return RedirectToAction(nameof(FriendsPage));
        }
    }
}
