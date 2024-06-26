﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
  /// <summary>
  /// Represents a video game entity.
  /// </summary>
  [Index(nameof(IgdbId), IsUnique = true)]
  public class Game
  {

    [Key]
    public long GameId { get; set; }
    /// <summary>
    /// Gets or sets the IGDB ID of the game.
    /// </summary>

    public long IgdbId { get; set; }

    public int? TotalRatingCount { get; set; }
    /// <summary>
    /// Gets or sets the name of the game.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Gets or sets the summary or description of the game.
    /// </summary>
    public string? Summary { get; set; }
    /// <summary>
    /// Gets or sets the rating of the game from IGDB.
    /// </summary>
    public double IgdbRating { get; set; }
    /// <summary>
    /// Gets or sets the URL of the game's cover image.
    /// </summary>
    public string? ImageUrl { get; set; }
    /// <summary>
    /// Gets or sets the Screenshots of the game.
    /// </summary>
    public string[]? Screenshots { get; set; }
    /// <summary>
    /// Gets or sets the URL of the game's video trailer.
    /// </summary>
    public string? VideoUrl { get; set; }
    /// <summary>
    /// Gets or sets the rating of the game for our app.
    /// QV -> QuestVault
    /// </summary>
    public int QvRating { get; set; } = 0;
    /// <summary>
    /// Gets or sets the Release date.
    /// </summary>
    public DateTime? ReleaseDate { get; set; }
    /// <summary>
    /// Gets or sets the released status.
    /// </summary>
    public bool IsReleased { get; set; }
    // public bool IsReleased { get; set; }

    /// <summary>
    /// Gets or sets the list of genres associated with the game.
    /// Represents a many-to-many relationship with Genres.
    /// </summary>
    public List<GameGenre>? GameGenres { get; set; }

    /// <summary>
    /// Gets or sets the list of platforms associated with the game.
    /// Represents a many-to-many relationship with Platforms.
    /// </summary>
    public List<GamePlatform>? GamePlatforms { get; set; }

    /// <summary>
    /// Gets or sets the list of companies associated with the game.
    /// Represents a many-to-many relationship with Companies.
    /// </summary>
    public List<GameCompany>? GameCompanies { get; set; }

    public string? SteamUrl { get; set; }
  }
}
