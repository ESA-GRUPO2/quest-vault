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
            Console.Out.WriteLine("aqui");
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

        // POST: api/Friendship/Request
        //[HttpPost, ActionName("SendFriendRequest")]
        public async Task<IActionResult> SendFriendRequestAsync(string id)
        {
            var receiver = await context.Users.FindAsync(id);
            var sender = await signInManager.UserManager.GetUserAsync(this.User);
            var friendshipRequest = new FriendshipRequest { };
            if (receiver != null && sender != null)
            {
                friendshipRequest.Receiver = receiver;
                friendshipRequest.Sender = sender;
                friendshipRequest.isAccepted = false;
                friendshipRequest.FriendshipDate = DateTime.Now;
                context.FriendshipRequest.Add(friendshipRequest);
                context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        //TO DO
        public async Task<IActionResult> AcceptFriendRequestAsync(string id)
        {
            return RedirectToAction(nameof(Index));
        }
        //TO DO
        public async Task<IActionResult> RejectFriendRequestAsync(string id)
        {
            return RedirectToAction(nameof(Index));
        }
        //TO DO
        public async Task<IActionResult> RemoveFriendAsync(string id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
