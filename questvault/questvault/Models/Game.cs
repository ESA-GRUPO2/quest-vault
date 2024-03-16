using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    /// <summary>
    /// Represents a video game entity.
    /// </summary>
    public class Game
    {

        [Key]
        public long GameId { get; set; }
        /// <summary>
        /// Gets or sets the name of the game.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the summary or description of the game.
        /// </summary>
        public string? Summary { get; set; }
        /// <summary>
        /// Gets or sets the rating of the game from IGDB.
        /// </summary>
        public double IgdbRating { get; set; }
        /// <summary>
        /// Gets or sets the URL of the game's cover image.
        /// </summary>
        public string? imageUrl { get; set; }
        /// <summary>
        /// Gets or sets the rating of the game for our app.
        /// QV -> QuestVault
        /// </summary>
        public double QvRating { get; set; }


        /// <summary>
        /// Gets or sets the list of genres associated with the game.
        /// Represents a many-to-many relationship with Genres.
        /// </summary>
        public List<GameGenre>? GameGenres { get; set; }

        /// <summary>
        /// Gets or sets the list of platforms associated with the game.
        /// Represents a many-to-many relationship with Platforms.
        /// </summary>
        //public virtual List<GamePlatform>? GamePlatform { get; set; }

        /// <summary>
        /// Gets or sets the list of companies associated with the game.
        /// Represents a many-to-many relationship with Companies.
        /// </summary>
        public List<GameCompany>? GameCompanies { get; set; }



    }
}
