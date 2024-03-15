namespace questvault.Models
{
    public class GameLog
    {
        public Game Game { get; set; }

        public int HoursPlayed { get; set; }

        public GameStatus Status { get; set; }

        public OwnageStatus Ownage {  get; set; }
        
    }

}
