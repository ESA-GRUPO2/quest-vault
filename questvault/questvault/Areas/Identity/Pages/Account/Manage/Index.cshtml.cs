// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using questvault.Models;
using System.ComponentModel.DataAnnotations;

namespace questvault.Areas.Identity.Pages.Account.Manage
{
  public class IndexModel(
      UserManager<User> userManager,
      SignInManager<User> signInManager,
      ILogger<IndexModel> logger) : PageModel
  {

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Username { get; set; }
    public string Email { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [StringLength(100, ErrorMessage = "This {0} already exits.")]
      [DataType(DataType.Text)]
      [Display(Name = "user name")]
      public string NewUserName { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "password")]
      public string OldPassword { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "new password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm new password")]
      [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
      public string ConfirmPassword { get; set; }
    }

    private async Task LoadAsync(User user)
    {
      var userName = await userManager.GetUserNameAsync(user);
      var email = await userManager.GetEmailAsync(user);

      Username = userName;
      Email = email;

      Input = new InputModel
      {
        NewUserName = userName
      };
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      await LoadAsync(user);
      return Page();
    }

    public async Task<IActionResult> OnPostUserNameAsync()
    {
      //foreach (var a in ModelState) await Console.Out.WriteLineAsync("model: " + a);
      var user = await userManager.GetUserAsync(User);
      if (user == null)
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");

      //if (Input.NewUserName == null) se rebentar a culpa é tua
      if (Input.NewUserName.Equals(null))
      {
        StatusMessage = "Empty field";
        return Page();
      }

    var userName = await userManager.GetUserNameAsync(user);
    if (Input.NewUserName != userName)
      {
        var setUserNameResult = await userManager.SetUserNameAsync(user, Input.NewUserName);
        if (!setUserNameResult.Succeeded)
        {
          StatusMessage = "Unexpected error when trying to update username.";
          return Page();
        }
      }

      await signInManager.RefreshSignInAsync(user);
      StatusMessage = "Your profile has been updated";
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostPasswordAsync()
    {
      foreach(var a in ModelState) await Console.Out.WriteLineAsync("model: "+a);
      if (!ModelState.IsValid)
      {
        StatusMessage = "Model is invalid";
        return Page();
      }

      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      var changePasswordResult = await userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
      if (!changePasswordResult.Succeeded)
      {
        foreach (var error in changePasswordResult.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return Page();
      }

      await signInManager.RefreshSignInAsync(user);
      logger.LogInformation("User changed their password successfully.");
      StatusMessage = "Your password has been changed.";

      return RedirectToPage();
    }
  }
}
