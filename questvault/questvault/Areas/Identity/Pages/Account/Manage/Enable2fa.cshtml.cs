// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using questvault.Models;

namespace questvault.Areas.Identity.Pages.Account.Manage
{
    public class Enable2faModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<Enable2faModel> _logger;

        public Enable2faModel(
            UserManager<User> userManager,
            ILogger<Enable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return Page();
            }
            throw new InvalidOperationException($"Cannot enable 2FA for user as it's currently enabled.");
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var enable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, true);
            if (!enable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred enabling 2FA.");
            }

            _logger.LogInformation("User with ID '{UserId}' has Enabled 2fa.", _userManager.GetUserId(User));
            StatusMessage = "2fa has been Enabled.";
            return RedirectToPage("./Index");
        }
    }
}
