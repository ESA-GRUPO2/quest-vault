﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace questvault.Models
{
    public class GameLog
    {
        [Key]
        public long GameLogId { get; set; }

        [ForeignKey("Game")]
        public long GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game? Game { get; set; }

        public long IgdbId { get; set; }

        [AllowNull]
        public int? HoursPlayed { get; set; }
        [AllowNull]
        public OwnageStatus Ownage {  get; set; }

        [AllowNull]
        public GameStatus Status { get; set; }

        [AllowNull]
        public int? Rating { get; set; }

        [AllowNull]
        public string? Review { get; set; }

    }
}
