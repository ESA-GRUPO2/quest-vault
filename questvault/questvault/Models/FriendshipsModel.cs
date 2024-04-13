using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
  /// <summary>
  /// Represents a model for user friendships.
  /// </summary>
  public class FriendshipsModel
    {
        [ForeignKey(nameof(UserId))]
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// Gets or sets the list of friendships.
    /// </summary>
    public List<Friendship>? FriendshipList { get; set; }
    /// <summary>
    /// Gets or sets the list of sent friendship requests.
    /// </summary>
    public List<FriendshipRequest>? SentFriendshipRequests { get; set; }
    /// <summary>
    /// Gets or sets the list of received friendship requests.
    /// </summary>
    public List<FriendshipRequest>? ReceivedFriendshipRequests { get; set; }
    }
}
