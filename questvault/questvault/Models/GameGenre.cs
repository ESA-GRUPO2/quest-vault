using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    [PrimaryKey(nameof(GameId), nameof(GenreId))]
    public class GameGenre
    {
        [Column(Order = 0)]
        public long GameId { get; set; }

        [Column(Order = 1)]
        public long GenreId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game? Game { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre? Genre { get; set; }
    }
}
