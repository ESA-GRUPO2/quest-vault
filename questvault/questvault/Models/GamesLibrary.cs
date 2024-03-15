using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class GamesLibrary
    {
        [Column(Order = 0)]
        public int UserID { get; set; }
        public List<GameLog> Games { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User? User { get; set; }

        public void AddGame(GameLog gameLog) { }

        public void UpdateGame(int gameId, GameLog gameLog) { }

        public void RemoveGame(int gameId) { }

        public String MostPlayedGenre()
        {
            return "";
        }

        public List<GameLog> Top5Games() 
        {
            return null;
        }

        public void SetTop5Games(List<GameLog> top5){ }

        public int TotalHoursPlayed() {  return 0; }

        public int NumberOfGamesStatus(GameStatus status) { return 0; }
    }
}
