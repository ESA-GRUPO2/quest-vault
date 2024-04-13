using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
  /// <summary>
  /// Represents an instance of a user login.
  /// </summary>
  public class LoginInstance
  {
    [Key]
    /// <summary>
    /// Gets or sets the ID of the login instance.
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// Gets or sets the login date.
    /// </summary>
    public DateOnly LoginDate { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the login instance.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    [Display(Name = "User")]
    public virtual User? User { get; set; }
  }
}
