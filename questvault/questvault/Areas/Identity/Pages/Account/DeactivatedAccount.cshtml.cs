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
  public class DeactivatedAccountModel(
    ApplicationDbContext dbContext,
    SignInManager<User> signInManager
    ) : PageModel
  {
    public IActionResult OnGet(string returnUrl, string UserId)
    {
      var user = dbContext.Users.FirstOrDefault(x => x.Id == UserId);
      returnUrl ??= Url.Content("~/");
      if (user == null) return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      if (!user.IsDeactivated) return RedirectToPage(returnUrl);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl, string UserId)
    {
      returnUrl ??= Url.Content("~/");
      User user = dbContext.Users.FirstOrDefault(u => u.Id == UserId);
      if (user == null) return base.NotFound($"Unable to load user with ID '{UserId}'.");
      user.ToggleActivation();
      dbContext.Update(user);
      await dbContext.SaveChangesAsync();
      var result = signInManager.SignInAsync(user, false);
      result.Wait();
      return RedirectToPage(returnUrl);
    }
  }
}
