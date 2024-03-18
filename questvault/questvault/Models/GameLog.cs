using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class GameLog
    {
        [Key]
        public long GameLogId { get; set; }

        [ForeignKey("Game")]
        public long GameID { get; set; }

        [ForeignKey(nameof(GameID))]
        public virtual Game? Game { get; set; }

        public int HoursPlayed { get; set; }

        public GameStatus Status { get; set; }

        public OwnageStatus Ownage {  get; set; }
       
    }
}
