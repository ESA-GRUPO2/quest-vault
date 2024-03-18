using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace questvault.Models
{
    public class FriendshipRequest
    {
        [ForeignKey("User")]
        public string SenderId { get; set; }

        [ForeignKey("User")]
        public string ReceiverId { get; set; }
        public bool isAccepted { get; set; } = false;
        public DateTime FriendshipDate { get; set; }

        // Propriedade de navegação
        [ForeignKey(nameof(SenderId))]
        [Display(Name = "Sender")]
        public virtual User? Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [Display(Name = "Receiver")]
        public virtual User? Receiver { get; set; }



    }
}
