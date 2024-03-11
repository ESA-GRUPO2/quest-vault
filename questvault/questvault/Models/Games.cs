using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class Games
    {

        [Key]
        public int GameID { get; set; }

        public string Name { get; set; }
        public List<string> Genres { get; set; }

    }
}
