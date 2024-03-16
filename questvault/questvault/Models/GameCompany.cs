using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    /// <summary>
    /// Represents a many-to-many relationship between Game and Company entities.
    /// </summary>
    [PrimaryKey (nameof(GameId), nameof(CompanyId))]
    public class GameCompany
    {
        /// <summary>
        /// Gets or sets the ID of the game.
        /// </summary>
        [Column(Order = 0)]
        public long GameId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the company.
        /// </summary>
        [Column(Order = 1)]
        public long CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the game.
        /// </summary>
        [ForeignKey(nameof(GameId))]
        public virtual Game? Game { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the company.
        /// </summary>
        [ForeignKey(nameof(CompanyId))]
        public virtual Company? Company { get; set; }
    }

}
