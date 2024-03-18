using System.ComponentModel.DataAnnotations;

namespace questvault.Models
{
    public class Friendship
    {
        public int id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }

        [Display(Name = "User1")]
        public User User1 { get; set; }

        [Display(Name = "User2")]
        public User User2 { get; set; }
    }
}
