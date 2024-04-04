using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace questvault.Models
{
  public class GameLog
  {
    /// <summary>
    /// Gets or sets the primary key for the game log.
    /// </summary>
    [Key]
    public long GameLogId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key for the associated game.
    /// </summary>
    [ForeignKey("Game")]
    public long GameId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property for the associated game.
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game? Game { get; set; }

    /// <summary>
    /// Gets or sets the IGDB (Internet Game Database) ID for the game.
    /// </summary>
    public long IgdbId { get; set; }

    /// <summary>
    /// Gets or sets the number of hours played for the game (nullable).
    /// </summary>
    [AllowNull]
    public int? HoursPlayed { get; set; }

    /// <summary>
    /// Gets or sets the ownage status for the game (nullable).
    /// </summary>
    [AllowNull]
    public OwnageStatus Ownage { get; set; }

    /// <summary>
    /// Gets or sets the status of the game (nullable).
    /// </summary>
    [AllowNull]
    public GameStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the rating given for the game (nullable).
    /// </summary>
    [AllowNull]
    public int? Rating { get; set; }

    /// <summary>
    /// Gets or sets the review for the game (nullable).
    /// </summary>
    [AllowNull]
    public string? Review { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
  }
}
