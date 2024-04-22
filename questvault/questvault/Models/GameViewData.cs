using questvault.Utils;

namespace questvault.Models
{
  /// <summary>
  /// Represents view data for games.
  /// </summary>
  public class GameViewData
  {
    /// <summary>
    /// Gets or sets the search term.
    /// </summary>
    public string SearchTerm { get; set; } = "";
    /// <summary>
    /// Gets or sets the number of results.
    /// </summary>
    public int NumberOfResults { get; set; }
    /// <summary>
    /// Gets or sets the list of games.
    /// </summary>
    public PaginatedList<Game>? Games { get; set; }
    /// <summary>
    /// Gets or sets the list of genres.
    /// </summary>
    public IEnumerable<Genre>? Genres { get; set; }

    /// <summary>
    /// Gets or sets the list of platforms.
    /// </summary>
    public IEnumerable<Platform>? Platforms { get; set; }





  }
}





