using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    [PrimaryKey (nameof(GameId), nameof(CompanyId))]
    public class GameCompany
    {
        [Column(Order = 0)]
        public long GameId { get; set; }

        [Column(Order = 1)]
        public long CompanyId { get; set; }
        [ForeignKey(nameof(GameId))]
        public virtual Game? Game { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company? Company { get; set; }

    }
}
