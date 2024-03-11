using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string? GenreName { get; set; }

        // Relacionamento Muitos-Para-Muitos com Games
        public List<GameGenre>? GamesGenres { get; set; }

    }
}
