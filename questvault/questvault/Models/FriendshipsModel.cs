using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class FriendshipsModel
    {
        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public List<Friendship>? FriendshipList { get; set; }
        public List<FriendshipRequest>? SentFriendshipRequests { get; set; }
        public List<FriendshipRequest>? ReceivedFriendshipRequests { get; set; }
    }
}
