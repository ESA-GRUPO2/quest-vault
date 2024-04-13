// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using questvault.Data;
using questvault.Models;

namespace questvault.Areas.Identity.Pages.Account.Manage
{
  public class TogglePrivacyModel(
        UserManager<User> userManager,
        ILogger<Disable2faModel> logger,
        ApplicationDbContext dbContext
        ) : PageModel

  {

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<bool> IsUserPrivateAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new InvalidOperationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      var account = await dbContext.Users.FindAsync(user.Id);
      if (account == null)
      {
        throw new InvalidOperationException($"Unable to find user account with ID '{user.Id}'.");
      }

      return account.IsPrivate;
    }

    public async Task<IActionResult> OnGet()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }



      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      var privateAccount = await dbContext.Users.FindAsync(user.Id);
      privateAccount.TogglePrivate();
      dbContext.Update(privateAccount);
      int a = dbContext.SaveChanges();
      if (a != 1)
      {
        throw new InvalidOperationException($"Unexpected error occurred changing privacy.");
      }

      logger.LogInformation("User with ID '{UserId}' has changed privacy.", userManager.GetUserId(User));
      StatusMessage = "Privacy has been changed";
      return RedirectToPage("./Index");
    }
  }
}
