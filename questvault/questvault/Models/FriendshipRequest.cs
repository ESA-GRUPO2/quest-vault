namespace questvault.Models
{
    public class FriendshipRequest
    {
        public int id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public bool isAccepted { get; set; } = false;
        public DateTime FriendshipDate { get; set; }

    }
}
