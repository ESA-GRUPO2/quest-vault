using Microsoft.AspNetCore.Identity;

namespace questvault.Models
{
  /// <summary>
  /// Represents a user in the application.
  /// </summary>
  public class User : IdentityUser
  {
    /// <summary>
    /// Gets or sets a value indicating whether the user is deactivated.
    /// </summary>
    public bool IsDeactivated { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether the user profile is private.
    /// </summary>
    public bool IsPrivate { get; set; } = false;
    /// <summary>
    /// Gets or sets the Steam ID associated with the user.
    /// </summary>
    public string? SteamID { get; set; }
    /// <summary>
    /// Gets or sets the path to the user's profile photo.
    /// </summary>
    public string? ProfilePhotoPath { get; set; }

    /// <summary>  
    /// Gets or sets the clearance level of the user.
    /// </summary>
    public int Clearance { get; set; } = 0;
    /// <summary>
    /// Toggles the activation status of the user.
    /// </summary>
    /// <returns>The updated activation status.</returns>
    public bool ToggleActivation() { return IsDeactivated = !IsDeactivated; }

    /// <summary>
    /// Toggles the privacy status of the user profile.
    /// </summary>
    /// <returns>The updated privacy status.</returns>
    public bool TogglePrivate() { return IsPrivate = !IsPrivate; }
    /// <summary>
    /// Upgrades the clearance level of the user.
    /// </summary>
    /// <returns>The updated clearance level.</returns>
    public int UpgradeClearance()
    {
      if (Clearance >= 2) return Clearance = 2;
      return ++Clearance;
    }

    /// <summary>
    /// Downgrades the clearance level of the user.
    /// </summary>
    /// <returns>The updated clearance level.</returns>
    public int DowngradeClearance()
    {
      if (Clearance <= 0) return Clearance = 0;
      return --Clearance;
    }
  }
}
