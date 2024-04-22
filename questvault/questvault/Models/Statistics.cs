using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace questvault.Models
{
  /// <summary>
  /// Represents statistical data.
  /// </summary>
  public class Statistics
  {
    /// <summary>
    /// Gets or sets the count of registered users.
    /// </summary>
    public int registeredUsersCount { get; set; }
    /// <summary>
    /// Gets or sets the average game rating.
    /// </summary>
    public double gameRatingAverage { get; set; }
    /// <summary>
    /// Gets or sets the list of login dates.
    /// </summary>
    public List<string> LoginDateList { get; set; }
    /// <summary>
    /// Gets or sets the count of logins per date.
    /// </summary>
    public List<int> LoginDateCountList { get; set; }
    /// <summary>
    /// Gets or sets the list of access dates.
    /// </summary>
    public List<string> AccessDateList { get; set; }
    /// <summary>
    /// Gets or sets the count of accesses per date.
    /// </summary>
    public List<int> AccessDateCountList { get; set; }
    /// <summary>
    /// Gets or sets the list of genre names.
    /// </summary>
    public List<string> GenreNames { get; set; }
    /// <summary>
    /// Gets or sets the count of games per genre.
    /// </summary>
    public List<int> GenreCount { get; set; }
  }
}
