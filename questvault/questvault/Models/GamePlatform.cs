using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    /// <summary>
    /// Represents a many-to-many relationship between Game and Platform entities.
    /// </summary>
    [PrimaryKey(nameof(GameId), nameof(PlatformId))]
    public class GamePlatform
    {
        /// <summary>
        /// Gets or sets the ID of the game.
        /// </summary>
        [Column(Order = 0)]
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the game.
        /// </summary>
        [ForeignKey(nameof(GameId))]
        public Game? Game { get; set; }

        /// <summary>
        /// Gets or sets the ID of the platform.
        /// </summary>
        [Column(Order = 1)]
        public long PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the platform.
        /// </summary>
        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; }
    }

}
