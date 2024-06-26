﻿using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    /// <summary>
    /// Represents a gaming genre entity.
    /// </summary>
    public class Genre
    {
        [Key]
        public long GenreId { get; set; }
        public long IgdbGenreId { get; set; }
        /// <summary>
        /// Gets or sets the name of the gaming genre.
        /// </summary>
        public string? GenreName { get; set; }

        /// <summary>
        /// Gets or sets the list of games associated with the genre.
        /// Represents a many-to-many relationship with Games.
        /// </summary>
        public List<GameGenre>? GameGenres { get; set; }

    }
}
