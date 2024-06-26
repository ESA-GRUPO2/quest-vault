﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        IWebHostEnvironment webHostEnvironment,
        BlobContainerClient blobContainer
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

    [Display(Name = "isPrivate")]
    public bool isPrivate { get; set; }

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
      var is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);
      var user1 = await userManager.GetUserAsync(User);

      Username = userName;
      Email = email;
      Is2faEnabled = is2faEnabled;
      isPrivate = user1.IsPrivate;
      var library = await context.GamesLibrary.
              Include(l => l.Top5Games).ThenInclude(gl => gl.Game).
              FirstOrDefaultAsync(l => l.UserId == user.Id);
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if( user == null )
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      await LoadAsync(user);
      ViewData["UserAvatar"] = user.ProfilePhotoPath;
      ViewData["Privacy"] = user.IsPrivate;
      return Page();
    }
    public async Task<IActionResult> OnPostProfileAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if( user == null ) return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      ViewData["Privacy"] = user.IsPrivate;
      foreach( var item in ModelState ) await Console.Out.WriteLineAsync($"item: {item}");
      if( !ModelState.IsValid )
      {
        ModelState.AddModelError(string.Empty, "Input your current password");
        return Page();
      }
      if( !Input.NewUserName.IsNullOrEmpty() )
      {
        if( Input.NewUserName.Equals(user.UserName) ) ModelState.AddModelError(string.Empty, "Can't change to same username");
        else
        {
          var setUserNameResult = await userManager.SetUserNameAsync(user, Input.NewUserName);
          if( !setUserNameResult.Succeeded ) ModelState.AddModelError(string.Empty, "This username is already taken");
          else
          {
            StatusMessage = "Your username has been updated. ";
          }
        }
      }
      if( !Input.NewPassword.IsNullOrEmpty() && !Input.ConfirmPassword.IsNullOrEmpty() )
      {
        var changePasswordResult = await userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if( !changePasswordResult.Succeeded )
        {
          foreach( var error in changePasswordResult.Errors ) ModelState.AddModelError(string.Empty, error.Description);
        }
        else if( StatusMessage == null ) StatusMessage = "Your password has been changed. "; else StatusMessage += "| Your password has been updated. ";
      }

      await signInManager.RefreshSignInAsync(user);
      await LoadAsync(user);
      return Page();
    }

    public async Task<IActionResult> OnPostAvatarAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if( user == null )
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      string filename = UploadFile(ProfilePhoto, user.Id);

      var userToUpdate = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
      if( userToUpdate != null )
      {
        userToUpdate.ProfilePhotoPath = "https://profilephotoblob.blob.core.windows.net/users-profile-photos/" + filename;
        await context.SaveChangesAsync();
      }

      return RedirectToPage();
    }

    private string UploadFile(IFormFile ProfilePhoto, string userId)
    {
      string fileName = null;
      if( ProfilePhoto != null && userId != null )
      {
        fileName = userId + ".jpg";
        try
        {
          blobContainer.UploadBlob(fileName, ProfilePhoto.OpenReadStream());
        }
        catch( Azure.RequestFailedException )
        {
          var client = blobContainer.GetBlobClient(fileName);
          client.Upload(ProfilePhoto.OpenReadStream(), true); // override = true
        }
      }
      return fileName;
    }
  }
}