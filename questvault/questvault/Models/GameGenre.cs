using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    [PrimaryKey(nameof(IgdbId), nameof(IgdbGenreId))]
    /// <summary>
    /// Represents a many-to-many relationship between Game and Genre entities.
    /// </summary>
    public class GameGenre
    {
        /// <summary>
        /// Gets or sets the ID of the game.
        /// </summary>
        [Column(Order = 0)]
        public long IgdbId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the genre.
        /// </summary>
        [Column(Order = 1)]
        public long IgdbGenreId { get; set; }
        /// <summary>
        /// Gets or sets the navigation property for the game.
        /// </summary>
        [ForeignKey(nameof(IgdbId))]
        public virtual Game? Game { get; set; }
        /// <summary>
        /// Gets or sets the navigation property for the genre.
        /// </summary>
        [ForeignKey(nameof(IgdbGenreId))]
        public virtual Genre? Genre { get; set; }
    }
}
