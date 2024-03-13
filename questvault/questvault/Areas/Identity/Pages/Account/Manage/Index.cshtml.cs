﻿// Licensed to the .NET Foundation under one or more agreements.
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

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool Is2faEnabled { get; set; }

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
      [Phone]
      [Display(Name = "Phone number")]
      public string PhoneNumber { get; set; }
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
      var phoneNumber = await userManager.GetPhoneNumberAsync(user);

      Username = userName;

      Input = new InputModel
      {
        PhoneNumber = phoneNumber
      };

      Is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);


    }

    public async Task<IActionResult> OnGetAsync()
    {

      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      Is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
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

      return Page();
    }
  }
}
