using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class Jogos
    {

        [Key]
        public int JogoId { get; set; }

        public string Nome { get; set; }
        public string Genero { get; set; }

       
    }
}
