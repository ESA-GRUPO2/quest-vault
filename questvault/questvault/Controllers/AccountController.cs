using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using questvault.Models;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;

    public AccountController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeUsername(string newUsername)
    {
        // Check if the new username is already taken
        var existingUser = await _userManager.FindByNameAsync(newUsername);
        if (existingUser != null)
        {
            ModelState.AddModelError(string.Empty, "Username is already taken.");
            return View();
        }

        // Get the current user
        var user = await _userManager.GetUserAsync(User);

        // Ensure user exists
        if (user == null)
        {
            return NotFound();
        }

        // Check if the new username is valid
        // Add more validation here
        if (string.IsNullOrWhiteSpace(newUsername))
        {
            ModelState.AddModelError(string.Empty, "Username cannot be empty");
            return View();
        }

        // Update the username
        user.UserName = newUsername;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // Username successfully updated
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Failed to update username, handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}

