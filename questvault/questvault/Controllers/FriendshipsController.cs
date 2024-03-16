using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
    public class FriendshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FriendshipRequests
        public async Task<IActionResult> IndexAsync()
        {
            var dbContext = await _context.FriendshipRequest.ToListAsync();
            return View(dbContext);
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

            var friendship = await _context.Friendship
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
                _context.Add(friendship);
                await _context.SaveChangesAsync();
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

            var friendship = await _context.Friendship.FindAsync(id);
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
                    _context.Update(friendship);
                    await _context.SaveChangesAsync();
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

            var friendship = await _context.Friendship
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
            var friendship = await _context.Friendship.FindAsync(id);
            if (friendship != null)
            {
                _context.Friendship.Remove(friendship);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }

        private bool FriendshipExists(int id)
        {
            return _context.Friendship.Any(e => e.id == id);
        }

        // POST: api/Friendship/Request
        [HttpPost("Request/{id}"), ActionName("SendFriendRequest")]
        public async Task<IActionResult> SendFriendRequestAsync(string id)
        {
            Console.WriteLine("Friend request sent");
            var receiver = await _context.Users.FindAsync(id);
            var sender = await _context.Users.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var friendshipRequest = new FriendshipRequest { };
            if (receiver != null && sender != null)
            {
                friendshipRequest.Receiver = receiver;
                friendshipRequest.Sender = sender;
                friendshipRequest.isAccepted = false;
                friendshipRequest.FriendshipDate = DateTime.Now;
                _context.FriendshipRequest.Add(friendshipRequest);
                _context.SaveChanges();
            }
            return Created();
        }
    }
}
