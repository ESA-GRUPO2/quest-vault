using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace questvault.Models
{
  public class GamesLibrary
  {
    /// <summary>
    /// Gets or sets the primary key for the games library.
    /// </summary>
    [Key]
    public long GamesLibraryId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key for the associated user.
    /// </summary>
    [ForeignKey("User")]
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property for the associated user.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    /// <summary>
    /// Gets or sets the list of game logs associated with this games library.
    /// </summary>
    public List<GameLog> GameLogs { get; set; }

    /// <summary>
    /// Gets or sets the top 5 games list associated with this games library (nullable).
    /// </summary>
    [AllowNull]
    public List<Game> Top5Games { get; set; }

  }
}
