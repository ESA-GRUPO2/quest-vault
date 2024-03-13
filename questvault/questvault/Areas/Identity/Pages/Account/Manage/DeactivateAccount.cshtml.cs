// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using questvault.Data;
using questvault.Models;

namespace questvault.Areas.Identity.Pages.Account
{
  /// <summary>
  ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
  ///     directly from your code. This API may change or be removed in future releases.
  /// </summary>
  [AllowAnonymous]
  public class DeactivateAccountModel(
    UserManager<User> userManager,
    ApplicationDbContext dbContext,
    SignInManager<User> signInManager
    ) : PageModel
  {
    public async Task<IActionResult> OnGetAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null) return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      if (user.IsDeactivated) throw new InvalidOperationException($"Cannot deactivate a deactivated account.");
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      User user = dbContext.Users.FirstOrDefault(u => u.Id == userManager.GetUserId(User));
      if (user == null) return base.NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      user.ToggleActivation();
      dbContext.Update(user);
      await dbContext.SaveChangesAsync();
      var result = signInManager.SignOutAsync();
      result.Wait();
      return RedirectToPage("Home");
    }
  }
}
