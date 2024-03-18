using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace questvault.Models
{
    /// <summary>
    /// Represents a gaming platform entity.
    /// </summary>
    public class Platform
    {
        [Key]
        public long PlatformId { get; set; }
        public long IgdbPlatformId { get; set; }
        /// <summary>
        /// Gets or sets the name of the gaming platform.
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// Gets or sets the list of games associated with the platform.
        /// Represents a many-to-many relationship with Games.
        /// </summary>
        public List<GamePlatform>? GamePlatforms { get; set; }

    }
}
