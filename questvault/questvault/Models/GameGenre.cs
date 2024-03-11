using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace questvault.Models
{
    [PrimaryKey(nameof(GamesID), nameof(GenresID))]
    public class GameGenre
    {

        [Column(Order = 0)]
        public int GamesID { get; set; }

        [Column(Order = 1)]
        public int GenresID { get; set; }

        [ForeignKey(nameof(GamesID))]
        public virtual Game? Game { get; set; }

        [ForeignKey(nameof(GenresID))]
        public virtual Genre? Genre { get; set; }
    }
}
