// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using IGDB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;
using System.ComponentModel.DataAnnotations;

namespace questvault.Areas.Identity.Pages.Account
{
    public class LoginModel(
        SignInManager<User> signInManager,
        ILogger<LoginModel> logger,
        UserManager<User> userManager,
        IUserStore<User> userStore,
        ApplicationDbContext context,
        IEmailSender emailSender
      ) : PageModel
    {
        private readonly IUserEmailStore<User> emailStore =
                userManager.SupportsUserEmail ?
                (IUserEmailStore<User>)userStore :
                throw new NotSupportedException("the deffault ui requires a user store with email support");
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            public string EmailUserName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToPage(returnUrl);


            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      if (ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(Input.EmailUserName));
        user ??= await context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(Input.EmailUserName));
        if (user == null)
        {
          Console.WriteLine("in: " + Input.EmailUserName);
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return Page();
        }
        if (user.IsDeactivated)
        {
          return RedirectToPage("./DeactivatedAccount", new { ReturnUrl = returnUrl, UserId = user.Id });
        }
        if (user.LockoutEnabled)
        {
          logger.LogWarning("User account locked out.");
          return RedirectToPage("./Lockout");
        }
        var result = await signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          logger.LogInformation("User logged in.");
          //login
          var logginInstance = new LoginInstance() { UserId = user.Id, LoginDate = DateOnly.FromDateTime(DateTime.Now) };
          context.LogginInstances.Add(logginInstance);
          await context.SaveChangesAsync();
          return RedirectToAction("UserLibrary", "Library", new { id = user.UserName });
        }
        if (result.RequiresTwoFactor)
        {
          //send email
          TwoFactorAuthenticationTokens twoFactorAuthenticator = new() { UserId = user.Id, User = user };
          await emailStore.SetEmailAsync(twoFactorAuthenticator.User, user.Email, CancellationToken.None);
          await emailSender.SendEmailAsync(user.Email, "Login Code", $"Your code to login is:\n\t" + twoFactorAuthenticator.Token);
          //registar token na db 
          if (context.EmailTokens.Any(t => t.UserId == user.Id)) context.Update(twoFactorAuthenticator);
          else context.Add(twoFactorAuthenticator);
          await context.SaveChangesAsync();
          return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return Page();
        }
      }
      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
