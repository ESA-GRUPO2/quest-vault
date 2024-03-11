using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace questvault.Models

{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string? CompanyName { get; set; }

        // Relacionamento Muitos-Para-Muitos com Games
        public virtual List<GameCompany>? GameCompany { get; set; }
    }
}
