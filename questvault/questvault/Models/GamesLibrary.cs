using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace questvault.Models
{
    public class GamesLibrary
    {
        [Key]
        public int GamesLibraryId { get; set; }

        [Column(Order = 0)]
        [ForeignKey("User")]
        public string UserID { get; set; }

        [AllowNull]
        public List<GameLog> GameLogs { get; set; }

        [AllowNull]
        public List<GameLog> Top5Games { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User? User { get; set; }

        public void AddGame(GameLog gameLog) {
            GameLogs.Add(gameLog);
        }

        public void UpdateGame(long gameId, GameLog gameLog) { }

        public void RemoveGame(int gameId) {
            foreach (GameLog gameLog in GameLogs)
            {
                if (gameLog.Game.GameId == gameId)
                {
                    GameLogs.Remove(gameLog);
                }
            }
        }

        public String MostPlayedGenre()
        {
            Dictionary<string, int> genre = new Dictionary<string, int>();

            return "";
        }

        public void AddTop5Games(GameLog gameLog)

        {
            Top5Games.Add(gameLog);
        }

        public void RemoveTop5Games(int gameId)
        {
            Top5Games.RemoveAt(gameId);
        }

        public int TotalHoursPlayed()
        { 
            int hours = 0;
            foreach (GameLog gameLog in GameLogs)
            {
                hours += gameLog.HoursPlayed;
            }
            return hours;
        }

        public int NumberOfGamesStatus(GameStatus status) 
        {
            int nStatus = 0;
            foreach (GameLog gameLog in GameLogs)
            {
                if (gameLog.Status.Equals(status))
                {
                    nStatus++;
                }
            }
            return nStatus;
        }
    }
}
