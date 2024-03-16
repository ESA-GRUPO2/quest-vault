using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class GameLog
    {

        [Column(Order = 0)]
        public int GamesID { get; set; }

        [ForeignKey(nameof(GamesID))]
        public virtual Game? Game { get; set; }

        public int HoursPlayed { get; set; }

        public GameStatus Status { get; set; }

        public OwnageStatus Ownage {  get; set; }
       
    }

}
