using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace questvault.Models
{
    public class FriendshipRequest
    {
        public int id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public bool isAccepted { get; set; } = false;
        public DateTime FriendshipDate { get; set; }

        // Propriedade de navegação
        [Display(Name = "Sender")]
        public virtual User? Sender { get; set; }

        [Display(Name = "Receiver")]
        public virtual User? Receiver { get; set; }



    }
}
