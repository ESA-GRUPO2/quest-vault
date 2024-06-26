﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace questvault.Models

{
    /// <summary>
    /// Represents a video game company entity.
    /// </summary>
    [Index(nameof(IgdbCompanyId), IsUnique = true)]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CompanyId { get; set; }
        public long IgdbCompanyId { get; set; }
        /// <summary>
        /// Gets or sets the name of the game company.
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the list of games associated with the company.
        /// Represents a many-to-many relationship with Games.
        /// </summary>
        public List<GameCompany>? GameCompanies { get; set; }
    }
}
