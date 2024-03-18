using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class Friendship
    {
        public string User1Id { get; set; }

        public string User2Id { get; set; }

        // Propriedade de navegação
        [ForeignKey(nameof(User1Id))]
        [Display(Name = "User1")]
        public virtual User? User1 { get; set; }

        // Propriedade de navegação
        [ForeignKey(nameof(User2Id))]
        [Display(Name = "User2")]
        public virtual User? User2 { get; set; }
    }
}
