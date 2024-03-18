using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    /// <summary>
    /// Represents a many-to-many relationship between Game and Company entities.
    /// </summary>
    [PrimaryKey (nameof(IgdbId), nameof(IgdbCompanyId))]
    public class GameCompany
    {
        /// <summary>
        /// Gets or sets the ID of the game.
        /// </summary>
        [Column(Order = 0)]
        public long IgdbId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the company.
        /// </summary>
        [Column(Order = 1)]
        public long IgdbCompanyId { get; set; }


        /// <summary>
        /// Gets or sets the navigation property for the game.
        /// </summary>
        [ForeignKey(nameof(IgdbId))]
        public virtual Game? Game { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the company.
        /// </summary>
        [ForeignKey(nameof(IgdbCompanyId))]
        public virtual Company? Company { get; set; }
    }

}
