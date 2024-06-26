﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using questvault.Models;
using questvault.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace questvault.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ExternalLoginModel : PageModel
  {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IUserEmailStore<User> _emailStore;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ExternalLoginModel> _logger;
    private readonly ApplicationDbContext _context;

    public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _userStore = userStore;
      _emailStore = GetEmailStore();
      _logger = logger;
      _emailSender = emailSender;
      _context = context;
    }

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
    public string ProviderDisplayName { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

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
      [Required]
      [EmailAddress]
      public string Email { get; set; }
    }

    public IActionResult OnGet() => RedirectToPage("./Login");

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
      // Request a redirect to the external login provider.
      var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
      var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");
      if (remoteError != null)
      {
        ErrorMessage = $"Error from external provider: {remoteError}";
        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      }
      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        ErrorMessage = "Error loading external login information.";
        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      }

      var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
      if (user == null)
      {
        Console.WriteLine("in: " + Input.Email);
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
      }
      if (user.IsDeactivated)
      {
        return RedirectToPage("./DeactivatedAccount", new { ReturnUrl = returnUrl, UserId = user.Id });
      }
      if (user.LockoutEnabled)
      {
        _logger.LogWarning("User account locked out.");
        return RedirectToPage("./Lockout");
      }
      // Sign in the user with this external login provider if the user already has a login.
      var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
      _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
      if (result.Succeeded)
      {

        var logginInstance = new LoginInstance() { UserId = user.Id, LoginDate = DateOnly.FromDateTime(DateTime.Now) };
        _context.LogginInstances.Add(logginInstance);
        await _context.SaveChangesAsync();
        //return LocalRedirect(returnUrl);
        return RedirectToAction("UserLibrary", "Library", new { id = _signInManager.UserManager.GetUserName(User) });
      }
      if (result.IsLockedOut)
      {
        return RedirectToPage("./Lockout");
      }
      else
      {

        // // If the user does not have an account, then ask the user to create an account.
        // ReturnUrl = returnUrl;
        // ProviderDisplayName = info.ProviderDisplayName;
        // if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        // {
        //     Input = new InputModel
        //     {
        //         Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        //     };
        // }
        // return Page();

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        ProviderDisplayName = info.ProviderDisplayName;
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
          Input = new InputModel
          {
            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
          };
        }

        // Check if the user already exists by email
        var existingUser = await _userManager.FindByEmailAsync(Input.Email);
        if (existingUser == null)
        {
          // User does not exist, create a new account (email activated)
          var newUser = new User { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true, LockoutEnabled = false };
          newUser.LockoutEnabled = false;
          var createResult = await _userManager.CreateAsync(newUser);

          if (createResult.Succeeded)
          {
            // Add external login to the new user
            var addLoginResult = await _userManager.AddLoginAsync(newUser, info);

            if (addLoginResult.Succeeded)
            {
              // // Sign in the new user
              // await _signInManager.SignInAsync(newUser, isPersistent: false);
              // return LocalRedirect(returnUrl);

              // Redirect the user to the SetPassword page for initial password setup
              await _signInManager.SignInAsync(newUser, isPersistent: false);
              return Redirect("/Identity/Account/Manage/SetPassword");

              //return RedirectToAction("SetPassword", "Manage", new { area = "Identity" });
            }
          }
          else
          {
            foreach (var error in createResult.Errors)
            {
              ModelState.AddModelError(string.Empty, error.Description);
            }
          }
        }
        else //user exists
        {
          existingUser.EmailConfirmed = true;
          // Add external login to the new user
          var addLoginResult = await _userManager.AddLoginAsync(existingUser, info);

          if (addLoginResult.Succeeded)
          {
            // Set email to confirmed
            // Sign in the new user
            await _signInManager.SignInAsync(existingUser, isPersistent: false);
            return LocalRedirect(returnUrl);

            //// Redirect the user to the SetPassword page for initial password setup
            //await _signInManager.SignInAsync(newUser, isPersistent: false);
            //return Redirect("/Identity/Account/Manage/SetPassword");

            ////return RedirectToAction("SetPassword", "Manage", new { area = "Identity" });
          }
        }
        return Page();


      }
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");
      // Get the information about the user from the external login provider
      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        ErrorMessage = "Error loading external login information during confirmation.";
        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      }

      if (ModelState.IsValid)
      {
        var user = CreateUser();
        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await _userManager.AddLoginAsync(user, info);
          if (result.Succeeded)
          {
            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var isLockedOut = await _userManager.SetLockoutEnabledAsync(user, false);
            user.LockoutEnabled = false;
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code, isLockedOut },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            // If account confirmation is required, we need to show the link if we don't have a real email sender
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
              return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
            }

            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
            return LocalRedirect(returnUrl);
          }
        }
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }

      ProviderDisplayName = info.ProviderDisplayName;
      ReturnUrl = returnUrl;
      return Page();
    }

    private User CreateUser()
    {
      try
      {
        return Activator.CreateInstance<User>();
      }
      catch
      {
        throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
            $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
            $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
      }
    }

    private IUserEmailStore<User> GetEmailStore()
    {
      if (!_userManager.SupportsUserEmail)
      {
        throw new NotSupportedException("The default UI requires a user store with email support.");
      }
      return (IUserEmailStore<User>)_userStore;
    }
  }
}
