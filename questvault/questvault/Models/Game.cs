using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class Game
    {

        [Key]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }

        // Relacionamento Muitos-Para-Muitos com Genres
       
        public List<GameGenre>? GamesGenres { get; set; }

        //// Relacionamento Muitos-Para-Muitos com Platforms
        public virtual List<GamePlatform>? GamePlatform { get; set; }

        //// Relacionamento Muitos-Para-Muitos com Companies
        public virtual List<GameCompany>? GameCompany { get; set; }

        public double Rating { get; set; }


    }
}
