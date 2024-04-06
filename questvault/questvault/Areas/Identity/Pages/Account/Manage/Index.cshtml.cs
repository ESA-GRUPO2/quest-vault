// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;
using System.ComponentModel.DataAnnotations;

namespace questvault.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel(
          UserManager<User> userManager,
          SignInManager<User> signInManager,
          ILogger<IndexModel> logger,
          ApplicationDbContext context,
          IWebHostEnvironment webHostEnvironment
      ) : PageModel
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
        public string Email { get; set; }

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
        /// 

        [Display(Name = "avatar")]
        public IFormFile ProfilePhoto { get; set; }

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

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await userManager.GetUserNameAsync(user);
            var email = await userManager.GetEmailAsync(user);
            var is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            Username = userName;
            Email = email;
            Is2faEnabled = is2faEnabled;
            var library = await context.GamesLibrary.
              Include(l => l.Top5Games).ThenInclude(gl => gl.Game).
              FirstOrDefaultAsync(l => l.UserId == user.Id);
            ViewData["Top5"] = null;
            if (library != null) { ViewData["Top5"] = library.Top5Games; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            ViewData["UserAvatar"] = user.ProfilePhotoPath;
            return Page();
        }

        public async Task<IActionResult> OnPostUserNameAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            if (string.IsNullOrEmpty(Input.NewUserName))
            {
                ModelState.AddModelError(string.Empty, "User name cannot be empty");
                await LoadAsync(user);
                return Page();
            }

            var userName = user.UserName;
            if (Input.NewUserName.Equals(userName))
            {
                ModelState.AddModelError(string.Empty, "Can't change to same username");
                await LoadAsync(user);
                return Page();
            }

            if (!Input.NewUserName.Equals(userName))
            {
                var setUserNameResult = await userManager.SetUserNameAsync(user, Input.NewUserName);
                if (!setUserNameResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "This username is already taken");
                    await LoadAsync(user);
                    return Page();
                }
            }

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your username has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Fields cannot be empty");
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

        public async Task<IActionResult> OnPostAvatarAsync()
        {
            string filename = UploadFile(ProfilePhoto);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var userToUpdate = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.ProfilePhotoPath = filename;
                await context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        private string UploadFile(IFormFile ProfilePhoto)
        {
            string fileName = null;
            if (ProfilePhoto == null)
            {
            }
            if (ProfilePhoto != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "img/usr_profile");
                fileName = Guid.NewGuid().ToString() + "_" + ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    ProfilePhoto.CopyTo(filestream);
                }
            }
            return fileName;
        }
    }
}