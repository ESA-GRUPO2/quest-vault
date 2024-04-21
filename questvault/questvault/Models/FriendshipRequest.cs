using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace questvault.Models
{
  /// <summary>
  /// Represents a friendship request sent by one user to another.
  /// </summary>
  public class FriendshipRequest
  {
    /// <summary>
    /// Gets or sets the ID of the sender.
    /// </summary>
    [ForeignKey("User")]
    public string SenderId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the receiver.
    /// </summary>
    [ForeignKey("User")]
    public string ReceiverId { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the friendship request is accepted.
    /// </summary>
    public bool isAccepted { get; set; } = false;
    /// <summary>
    /// Gets or sets the date of the friendship request.
    /// </summary>
    public DateTime FriendshipDate { get; set; }
    /// <summary>
    /// Gets or sets the sender of the friendship request.
    /// </summary>

    [ForeignKey(nameof(SenderId))]
    [Display(Name = "Sender")]
    public virtual User? Sender { get; set; }
    /// <summary>
    /// Gets or sets the receiver of the friendship request.
    /// </summary>
    [ForeignKey(nameof(ReceiverId))]
    [Display(Name = "Receiver")]
    public virtual User? Receiver { get; set; }



  }
}
