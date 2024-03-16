using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace questvault.Models
{
    [PrimaryKey(nameof(GameId), nameof(PlatformId))]
    public class GamePlatform
    {
        [Column(Order = 0)]
        public long GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game? Game { get; set; }
        
        [Column(Order = 1)]
        public long PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; }
    }
}
