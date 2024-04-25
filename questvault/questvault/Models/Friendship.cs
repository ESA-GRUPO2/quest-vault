using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
  /// <summary>
  /// Represents a friendship between two users.
  /// </summary>
  public class Friendship
  {
    /// <summary>
    /// Gets or sets the ID of the first user.
    /// </summary>
    public string User1Id { get; set; }
    /// <summary>
    /// Gets or sets the ID of the second user.
    /// </summary>
    public string User2Id { get; set; }

    /// <summary>
    /// Gets or sets the first user.
    /// </summary>
    [ForeignKey(nameof(User1Id))]
    [Display(Name = "User1")]
    public virtual User? User1 { get; set; }

    /// <summary>
    /// Gets or sets the second user.
    /// </summary>
    [ForeignKey(nameof(User2Id))]
    [Display(Name = "User2")]
    public virtual User? User2 { get; set; }
  }
}
