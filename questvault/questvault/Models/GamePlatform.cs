﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace questvault.Models
{

    [PrimaryKey(nameof(GameID), nameof(PlatformID))]
    public class GamePlatform
    {
        [Column(Order = 0)]
        public int GameID { get; set; }

        [Column(Order = 1)]
        public int PlatformID { get; set; }

        [ForeignKey(nameof(GameID))]
        public Game? Game { get; set; }

        [ForeignKey(nameof(PlatformID))]
        public Platform? Platform { get; set; }

    }
}
