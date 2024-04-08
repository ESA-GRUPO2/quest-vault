using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
         private readonly UserController userController;

    public async Task<IActionResult> GiveModeratorAll(string id)
        {
            await GiveModerator(id, context);

            // Redirecione para alguma página após a conclusão
            return RedirectToAction("AllUsers", "Home");
        }

        public async Task<IActionResult> GiveModerator(string id)
        {
          await GiveModerator(id, context);

         
          // Redirecione para alguma página após a conclusão
          return RedirectToAction("Profile", "User", new { name1= signInManager.UserManager.GetUserName(User) , id2= id});
        }

    public static async Task GiveModerator(string id, ApplicationDbContext context)
        {
          if (id == null)
          {
            return; // Retorna NotFound se o ID do usuário não for fornecido
          }

          // Encontre o usuário pelo ID
          var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

          if (user == null)
          {
            return; // Retorna NotFound se o usuário não for encontrado
          }

          // Atualize o campo Clearance do usuário para 1
          user.Clearance = 1;

          // Salve as alterações no banco de dados
          await context.SaveChangesAsync();
        }

      public static async Task GiveAdmin(string id, ApplicationDbContext context)
      {
        if (id == null)
        {
          return; // Retorna NotFound se o ID do usuário não for fornecido
        }

        // Encontre o usuário pelo ID
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
          return; // Retorna NotFound se o usuário não for encontrado
        }

        // Atualize o campo Clearance do usuário para 1
        user.Clearance = 2;

        // Salve as alterações no banco de dados
        await context.SaveChangesAsync();
      }

      public async Task<IActionResult> GiveAdminAll(string id)
        {
            await GiveAdmin(id, context);

            // Redirecione para alguma página após a conclusão
            return RedirectToAction("AllUsers", "Home");
        }

      public async Task<IActionResult> GiveAdmin(string id)
      {
        await GiveAdmin(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Profile", "User", new { name1 = signInManager.UserManager.GetUserName(User), id2 = id });
    }

    public static async Task RemovePermissions(string id, ApplicationDbContext context)
    {
      if (id == null)
      {
        return; // Retorna NotFound se o ID do usuário não for fornecido
      }

      // Encontre o usuário pelo ID
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if (user == null)
      {
        return; // Retorna NotFound se o usuário não for encontrado
      }

      // Atualize o campo Clearance do usuário para 1
      user.Clearance = 0;

      // Salve as alterações no banco de dados
      await context.SaveChangesAsync();
    }
    public async Task<IActionResult> RemovePermissionsAll(string id)
        {
            await RemovePermissions(id, context);
            // Redirecione para alguma página após a conclusão
            return RedirectToAction("AllUsers", "Home");
    }

    public async Task<IActionResult> RemovePermissions(string id)
    {
      await RemovePermissions(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Profile", "User", new { name1 = signInManager.UserManager.GetUserName(User), id2 = id });
    }

    public static async Task LockoutUser(string id, ApplicationDbContext context)
    {
      if (id == null)
      {
        return; // Retorna NotFound se o ID do usuário não for fornecido
      }

      // Encontre o usuário pelo ID
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if (user == null)
      {
        return; // Retorna NotFound se o usuário não for encontrado
      }

      user.LockoutEnabled = true;

      // Salve as alterações no banco de dados
      await context.SaveChangesAsync();
    }

    public static async Task RemoveLockoutUser(string id, ApplicationDbContext context)
    {
      if (id == null)
      {
        return; // Retorna NotFound se o ID do usuário não for fornecido
      }

      // Encontre o usuário pelo ID
      var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if (user == null)
      {
        return; // Retorna NotFound se o usuário não for encontrado
      }

      user.LockoutEnabled = false;

      // Salve as alterações no banco de dados
      await context.SaveChangesAsync();
    }

    public async Task<IActionResult> LockoutUserAll(string id)
    {
      await LockoutUser(id, context);
      // Redirecione para alguma página após a conclusão
      return RedirectToAction("AllUsers", "Home");
    }

    public async Task<IActionResult> LockoutUser(string id)
    {
      await LockoutUser(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Profile", "User", new { name1 = signInManager.UserManager.GetUserName(User), id2 = id });
    }

    public async Task<IActionResult> RemoveLockoutUserAll(string id)
    {
      await RemoveLockoutUser(id, context);
      // Redirecione para alguma página após a conclusão
      return RedirectToAction("AllUsers", "Home");
    }

    public async Task<IActionResult> RemoveLockoutUser(string id)
    {
      await RemoveLockoutUser(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Profile", "User", new { name1 = signInManager.UserManager.GetUserName(User), id2 = id });
    }

    public static async Task RemoveReview(long gameLogId, ApplicationDbContext context)
    {
        // Atualize a classificação (se necessário)
        var gameLog = await context.GameLog.FirstOrDefaultAsync(g => g.GameLogId == gameLogId);
        if (gameLog != null)
        {
          gameLog.Rating = null; // Defina a classificação como 0 (ou qualquer outro valor apropriado)
          gameLog.Review = null;
        }

        // Salve as alterações no banco de dados
        await context.SaveChangesAsync();
    }

    public async Task<IActionResult> RemoveReview(long id)
    {
      if (id == 0)
      {
        return NotFound(); // Retorna NotFound se o ID do usuário não for fornecido
      }

      var gameLog = await context.GameLog.FirstOrDefaultAsync(g => g.GameLogId == id);
      if (gameLog == null) {
        return NotFound();
      }

      await RemoveReview(id, context);

      // Redirecione para alguma página após a conclusão
      return RedirectToAction("Details", "Games", new { id = gameLog.IgdbId });
    }
  }

}
