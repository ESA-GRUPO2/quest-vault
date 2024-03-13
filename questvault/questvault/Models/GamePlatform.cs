using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace questvault.Models
{

    [PrimaryKey(nameof(GameID), nameof(PlatformID))]
    /// <summary>
    /// Represents a many-to-many relationship between Game and Platform entities.
    /// </summary>
    public class GamePlatform
    {
        [Column(Order = 0)]
        public int GameID { get; set; }

        [Column(Order = 1)]
        public int PlatformID { get; set; }

        [ForeignKey(nameof(GameID))]
        public Game? Game { get; set; }

        [ForeignKey(nameof(PlatformID))]
        public Platform? Platform { get; set; }

    }
}
