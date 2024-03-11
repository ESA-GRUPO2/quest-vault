using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    public class Games
    {

        [Key]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }

        // Relacionamento Muitos-Para-Muitos com Genres
       
        public List<GamesGenres> GamesGenres { get; set; }

        //// Relacionamento Muitos-Para-Muitos com Platforms
        //public virtual List<Platform>? Platforms { get; set; }

        //// Relacionamento Muitos-Para-Muitos com Companies
        //public virtual List<Company>? Companies { get; set; }

        public double Rating { get; set; }


    }
}
