using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace questvault.Models
{
    public class Platform
    {
        [Key]
        public int PlatformID { get; set; }
        public string PlatformName { get; set; }

        // Relacionamento Muitos-Para-Muitos com Games
    
        public virtual List<Games>? Games { get; set; }

    }
}
