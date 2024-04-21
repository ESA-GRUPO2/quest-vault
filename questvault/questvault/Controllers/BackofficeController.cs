using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
//using questvault.Migrations;
using questvault.Services;

namespace questvault.Controllers
{

  public class BackofficeController(ApplicationDbContext context, IServiceIGDB igdbService, SignInManager<User> signInManager) : Controller
  {

    /// <summary>
    /// Grants moderator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant moderator permissions.</param>
    /// <returns>A redirection to the AllUsers page.</returns>
    public async Task<IActionResult> MakeModeratorAll(string id)
    {
      await MakeModerator(id, context);


      return RedirectToAction("AllUsers", "Home");
    }

    /// <summary>
    /// Grants moderator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant moderator permissions.</param>
    /// <returns>A redirection to the user's profile page.</returns>
    public async Task<IActionResult> MakeModerator(string id)
    {
      await MakeModerator(id, context);
      return RedirectToAction("Profile", "User", new { id });
    }

    /// <summary>
    /// Grants moderator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant moderator permissions.</param>
    /// <param name="context">The application database context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task MakeModerator(string id, ApplicationDbContext context)
    {
      if( id == null )
      {
        return;
      }


      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( user == null )
      {
        return;
      }


      user.Clearance = 1;


      await context.SaveChangesAsync();
    }

    /// <summary>
    /// Grants administrator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant administrator permissions.</param>
    /// <param name="context">The application database context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task MakeAdmin(string id, ApplicationDbContext context)
    {
      if( id == null )
      {
        return;
      }


      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( user == null )
      {
        return;
      }

      user.Clearance = 2;


      await context.SaveChangesAsync();
    }


    /// <summary>
    /// Grants administrator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant administrator permissions.</param>
    /// <returns>A redirection to the AllUsers page.</returns>
    public async Task<IActionResult> MakeAdminAll(string id)
    {
      await MakeAdmin(id, context);

      return RedirectToAction("AllUsers", "Home");
    }

    /// <summary>
    /// Grants administrator permissions to a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to grant administrator permissions.</param>
    /// <returns>A redirection to the user's profile page.</returns>
    public async Task<IActionResult> MakeAdmin(string id)
    {
      await MakeAdmin(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Profile", "User", new { id });
    }
    /// <summary>
    /// Removes all permissions from a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove permissions from.</param>
    /// <param name="context">The application database context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task RemovePermissions(string id, ApplicationDbContext context)
    {
      if( id == null )
      {
        return;
      }


      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( user == null )
      {
        return;
      }


      user.Clearance = 0;


      await context.SaveChangesAsync();
    }
    /// <summary>
    /// Removes all permissions from a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove permissions from.</param>
    /// <returns>A redirection to the AllUsers page.</returns>
    public async Task<IActionResult> RemovePermissionsAll(string id)
    {
      await RemovePermissions(id, context);

      return RedirectToAction("AllUsers", "Home");
    }
    /// <summary>
    /// Removes all permissions from a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove permissions from.</param>
    /// <returns>A redirection to the user's profile page.</returns>
    public async Task<IActionResult> RemovePermissions(string id)
    {
      await RemovePermissions(id, context);


      return RedirectToAction("Profile", "User", new { id });
    }

    public static async Task LockoutUser(string id, ApplicationDbContext context)
    {
      if( id == null )
      {
        return;
      }


      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( user == null )
      {
        return;
      }

      user.LockoutEnabled = true;


      await context.SaveChangesAsync();
    }

    public static async Task RemoveLockoutUser(string id, ApplicationDbContext context)
    {
      if( id == null )
      {
        return;
      }

      // Encontre o usuário pelo ID
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( user == null )
      {
        return;
      }

      user.LockoutEnabled = false;


      await context.SaveChangesAsync();
    }
    /// <summary>
    /// Locks out a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to lock out.</param>
    /// <returns>A redirection to the AllUsers page.</returns>
    public async Task<IActionResult> LockoutUserAll(string id)
    {
      await LockoutUser(id, context);

      return RedirectToAction("AllUsers", "Home");
    }
    /// <summary>
    /// Locks out a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to lock out.</param>
    /// <returns>A redirection to the user's profile page.</returns>
    public async Task<IActionResult> LockoutUser(string id)
    {
      await LockoutUser(id, context);


      return RedirectToAction("Profile", "User", new { id });
    }
    /// <summary>
    /// Removes the lockout from a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove the lockout from.</param>
    /// <returns>A redirection to the AllUsers page.</returns>
    public async Task<IActionResult> RemoveLockoutUserAll(string id)
    {
      await RemoveLockoutUser(id, context);

      return RedirectToAction("AllUsers", "Home");
    }
    /// <summary>
    /// Removes the lockout from a user identified by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to remove the lockout from.</param>
    /// <returns>A redirection to the user's profile page.</returns>
    public async Task<IActionResult> RemoveLockoutUser(string id)
    {
      await RemoveLockoutUser(id, context);


      return RedirectToAction("Profile", "User", new { id });
    }
    /// <summary>
    /// Removes the review from a game log identified by its ID.
    /// </summary>
    /// <param name="gameLogId">The ID of the game log to remove the review from.</param>
    /// <param name="context">The application database context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task RemoveReview(long gameLogId, ApplicationDbContext context)
    {

      var gameLog = await context.GameLog.FirstOrDefaultAsync(g => g.GameLogId == gameLogId);
      if( gameLog != null )
      {
        gameLog.Rating = null;
        gameLog.Review = null;
      }


      await context.SaveChangesAsync();
    }
    /// <summary>
    /// Removes the review from a game log identified by its ID.
    /// </summary>
    /// <param name="id">The ID of the game log to remove the review from.</param>
    /// <returns>A redirection to the game's details page.</returns>
    public async Task<IActionResult> RemoveReview(long id)
    {
      if( id == 0 )
      {
        return NotFound();
      }

      var gameLog = await context.GameLog.FirstOrDefaultAsync(g => g.GameLogId == id);
      if( gameLog == null )
      {
        return NotFound();
      }

      await RemoveReview(id, context);


      return RedirectToAction("Details", "Games", new { id = gameLog.IgdbId });
    }
  }

}
