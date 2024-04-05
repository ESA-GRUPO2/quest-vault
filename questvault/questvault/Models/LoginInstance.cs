using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class LoginInstance
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string LoginDate { get; set; }

        // Propriedade de navegação
        [ForeignKey(nameof(UserId))]
        [Display(Name = "User")]
        public virtual User? User { get; set; }
    }
}
