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
        public async Task<IActionResult> GiveModerator(string id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna NotFound se o ID do usuário não for fornecido
            }

            // Encontre o usuário pelo ID
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Retorna NotFound se o usuário não for encontrado
            }

            // Atualize o campo Clearance do usuário para 1
            user.Clearance = 1;

            // Salve as alterações no banco de dados
            await context.SaveChangesAsync();

            // Redirecione para alguma página após a conclusão
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GiveAdmin(string id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna NotFound se o ID do usuário não for fornecido
            }

            // Encontre o usuário pelo ID
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Retorna NotFound se o usuário não for encontrado
            }

            // Atualize o campo Clearance do usuário para 1
            user.Clearance = 2;

            // Salve as alterações no banco de dados
            await context.SaveChangesAsync();

            // Redirecione para alguma página após a conclusão
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RemovePermissions(string id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna NotFound se o ID do usuário não for fornecido
            }

            // Encontre o usuário pelo ID
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); // Retorna NotFound se o usuário não for encontrado
            }

            // Atualize o campo Clearance do usuário para 1
            user.Clearance = 0;

            // Salve as alterações no banco de dados
            await context.SaveChangesAsync();

            // Redirecione para alguma página após a conclusão
            return RedirectToAction("Index", "Home");
        }

    }

}
