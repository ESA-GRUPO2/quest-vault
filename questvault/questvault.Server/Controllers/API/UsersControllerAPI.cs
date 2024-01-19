using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Server.Data;
using questvault.Server.Model;

namespace questvault.Server.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllerAPI : ControllerBase
    {
        private readonly questvaultServerContext _context;

        public UsersControllerAPI(questvaultServerContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: Users/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string? id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = user.UserID }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool UserExists(Guid id) => _context.User.Any(e => e.UserID == id);
    }
}
