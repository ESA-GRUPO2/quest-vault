using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace questvault.Models
{
    public class GamesLibrary
    {
        [Column(Order = 0)]
        public int UserID { get; set; }

        [AllowNull]
        public List<GameLog> Games { get; set; }

        [AllowNull]
        public List<GameLog> Top5Games { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User? User { get; set; }

        public void AddGame(GameLog gameLog) {
            Games.Add(gameLog);
        }

        public void UpdateGame(int gameId, GameLog gameLog) { }

        public void RemoveGame(int gameId) {
            Games.RemoveAt(gameId);
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
            foreach (GameLog gameLog in Games)
            {
                hours += gameLog.HoursPlayed;
            }
            return hours;
        }

        public int NumberOfGamesStatus(GameStatus status) 
        {
            int nStatus = 0;
            foreach (GameLog gameLog in Games)
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
