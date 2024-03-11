using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    [PrimaryKey(nameof(GameID), nameof(CompanyID))]
    public class GameCompany
    {
        [Column(Order = 0)]
        public int GameID { get; set; }

        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [ForeignKey(nameof(GameID))]
        public virtual Game? Game { get; set; }
        [ForeignKey(nameof(CompanyID))]
        public virtual Company? Company { get; set; }
    }
}
