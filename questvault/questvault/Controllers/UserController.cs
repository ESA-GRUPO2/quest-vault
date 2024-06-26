﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Data;
using questvault.Models;

namespace questvault.Controllers
{
  /// <summary>
  /// Controller responsible for managing user profiles.
  /// </summary>
  public class UserController(ApplicationDbContext context, SignInManager<User> signInManager) : Controller
  {
    /// <summary>
    /// Redirects to the public profile page of the specified user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The public profile view.</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile(string id)
    {
      return RedirectToAction("PublicProfile", "User", new { id = id });
    }

    /// <summary>
    /// Displays the public profile of the specified user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The public profile view.</returns>
    [Authorize]
    public async Task<IActionResult> PublicProfile(string id)
    {

      if( id == null )
      {
        return NotFound();
      }

      var profileData = await ProcessProfileData(id);

      if( profileData == null )
      {
        return NotFound();
      }

      if( !profileData.CanViewProfile || profileData.IsLockedOut )
      {
        return RedirectToAction("PrivateProfile", "User", new { id });
      }

      var verifiedProfileData = await GetProfileData(profileData);



      ViewData["Top5"] = await LibraryController.GetTop5(id, context);
      return View(verifiedProfileData);
    }

    /// <summary>
    /// Displays the private profile of the specified user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The private profile view.</returns>
    [Authorize]
    public async Task<IActionResult> PrivateProfile(string id)
    {

      if( id == null )
      {
        return NotFound();
      }

      var profileData = await ProcessProfileData(id);

      if( profileData == null )
      {
        return NotFound();
      }

      var verifiedProfileData = await GetProfileData(profileData);


      return View(verifiedProfileData);
    }

    /// <summary>
    /// Processes the profile data of the specified user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The profile data.</returns>
    private async Task<ProfileViewData> ProcessProfileData(string id)
    {

      bool friends = false;
      bool send = false;
      bool received = false;

      var friendships = await context.Friendship.ToListAsync();
      var requests = await context.FriendshipRequest.ToListAsync();
      var userLogged = await signInManager.UserManager.GetUserAsync(this.User);
      var userProfile = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

      if( userLogged == null || userProfile == null )
      {
        return null;
      }

      // Check for friendships
      foreach( var friendship in friendships )
      {
        if( ( friendship.User1 == userLogged && friendship.User2 == userProfile ) ||
            ( friendship.User1 == userProfile && friendship.User2 == userLogged ) )
        {
          friends = true;
          break;
        }
      }

      // Check for friend requests
      foreach( var request in requests )
      {
        if( request.SenderId == userLogged.Id && request.ReceiverId == userProfile.Id )
        {
          send = true;
        }
        else if( request.SenderId == userProfile.Id && request.ReceiverId == userLogged.Id )
        {
          received = true;
        }
      }
      return new ProfileViewData
      {
        Friends = friends,
        Send = send,
        Received = received,
        CanViewProfile = !( userProfile.IsPrivate && !friends && userLogged.Clearance == 0 && userLogged != userProfile ),
        IsLockedOut = userProfile.LockoutEnabled,
        Friendships = friendships,
        FriendshipsRequests = requests,
        UserLogged = userLogged,
        UserProfile = userProfile
      };

    }

    /// <summary>
    /// Retrieves the profile data of the specified user.
    /// </summary>
    /// <param name="profileData">The profile data.</param>
    /// <returns>The verified profile data.</returns>
    private async Task<FriendsViewData> GetProfileData(ProfileViewData profileData)
    {

      if( profileData.CanViewProfile )
      {
        var userGameLogs = context.GamesLibrary
        .Include(gl => gl.GameLogs)
        .ThenInclude(gl => gl.Game)
        .Where(gl => gl.User == profileData.UserProfile)
        .SelectMany(gl => gl.GameLogs)
        .ToList();

        var ratingCount = context.GameLog?
            .Where(gl => gl.Rating.HasValue)
            .Where(gl => gl.User == profileData.UserProfile)
            .GroupBy(gl => gl.Rating.Value)
            .Select(group => new { Rating = group.Key, Count = group.Count() }) // Seleciona a chave de agrupamento (Rating) e a contagem
            .ToList();

        var allRatingsCount = Enumerable.Range(1, 5) // Gera uma sequência de 1 a 5
            .Select(rating => new
            {
              Rating = rating,
              Count = ratingCount.FirstOrDefault(rc => rc.Rating == rating)?.Count ?? 0
            }) // Obtém a contagem do rating específico, se não encontrado, usa 0
            .OrderBy(r => r.Rating) // Garante a ordem pelo rating
            .Select(r => r.Count) // Seleciona apenas a contagem
            .ToList();

        var friendsViewData = new FriendsViewData
        {
          user = profileData.UserProfile,
          friends = profileData.Friends,
          RequestSent = profileData.Send,
          RequestRecieved = profileData.Received,
          nJogosTotal = userGameLogs.Count(),
          nJogosPlaying = userGameLogs.Where(gl => gl.Status == GameStatus.Playing).Count(),
          nJogosComplete = userGameLogs.Where(gl => gl.Status == GameStatus.Complete).Count(),
          nJogosRetired = userGameLogs.Where(gl => gl.Status == GameStatus.Retired).Count(),
          nJogosBacklogged = userGameLogs.Where(gl => gl.Status == GameStatus.Backlogged).Count(),
          nJogosAbandoned = userGameLogs.Where(gl => gl.Status == GameStatus.Abandoned).Count(),
          nJogosWishlist = userGameLogs.Where(gl => gl.Status == GameStatus.Wishlist).Count(),
          ratingCountList = allRatingsCount
        };
        return friendsViewData;
      }
      else
      {
        var friendsViewData = new FriendsViewData
        {
          user = profileData.UserProfile,
          friends = profileData.Friends,
          RequestSent = profileData.Send,
          RequestRecieved = profileData.Received,

        };
        return friendsViewData;
      }



    }

    /// <summary>
    /// Sends a friend request to the specified user.
    /// </summary>
    /// <param name="id">The ID of the user to send the request to.</param>
    /// <returns>The profile view.</returns>
    public async Task<IActionResult> SendFriendRequestAsync(string id)
    {
      await FriendshipsController.SendFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(id);
    }

    /// <summary>
    /// Accepts a friend request from the specified user.
    /// </summary>
    /// <param name="id">The ID of the user who sent the request.</param>
    /// <returns>The profile view.</returns>
    public async Task<IActionResult> AcceptFriendRequestAsync(string id)
    {
      await FriendshipsController.AcceptFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(id);
    }

    /// <summary>
    /// Rejects a friend request from the specified user.
    /// </summary>
    /// <param name="id">The ID of the user who sent the request.</param>
    /// <returns>The profile view.</returns>
    public async Task<IActionResult> RejectFriendRequestAsync(string id)
    {
      await FriendshipsController.RejectFriendRequestAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(id);
    }

    /// <summary>
    /// Removes a friend from the user's friend list.
    /// </summary>
    /// <param name="id">The ID of the friend to remove.</param>
    /// <returns>The profile view.</returns>
    public async Task<IActionResult> RemoveFriendAsync(string id)
    {
      await FriendshipsController.RemoveFriendAsync(signInManager.UserManager.GetUserId(User), id, context);
      return await Profile(id);
    }

    /// <summary>
    /// Represents the profile view data.
    /// </summary>
    private class ProfileViewData
    {
      public bool Friends { get; set; }
      public bool Send { get; set; }
      public bool Received { get; set; }
      public bool CanViewProfile { get; set; }
      public bool IsLockedOut { get; set; }
      public List<Friendship> Friendships { get; set; }
      public List<FriendshipRequest> FriendshipsRequests { get; set; }
      public User UserLogged { get; set; }
      public User UserProfile { get; set; }

    }
  }
}


