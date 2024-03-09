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
        if (!string.IsNullOrEmpty(newUsername))
        {
            var existingUser = await _userManager.FindByNameAsync(newUsername);
            if (existingUser != null)
            {
                ModelState.AddModelError("newUsername", "Username already exists.");
                return View(); // Return the view with validation errors
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.UserName = newUsername;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Username successfully updated
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }
        }
        else
        {
            ModelState.AddModelError("newUsername", "Username cannot be empty");
        }

        // If we're here, there was an error, so return to the view with errors
        return View();
    }
}
