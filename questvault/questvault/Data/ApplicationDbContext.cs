
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using questvault.Models;
using questvault.Services;
using System;
using System.Reflection.Emit;

namespace questvault.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceIGDB igdbService) : IdentityDbContext<User>(options)
    {
        public DbSet<TwoFactorAuthenticationTokens> EmailTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TwoFactorAuthenticationTokens>().HasKey(t => t.UserId);




            //// Get data from IGDB service
            var genres = igdbService.GetGenres().Result;

            var gamesList = igdbService.GetPopularGames(10).Result;

            var idscomp = gamesList.SelectMany(game =>
                game.GameCompanies.Select(comp => comp.CompanyId)).Distinct().ToList();

            var idsplat = gamesList.SelectMany(game =>
                game.GamePlatforms.Select(p => p.PlatformId)).Distinct().ToList();

            var companies = igdbService.GetCompaniesFromIds(idscomp).Result;
            var platforms = igdbService.GetPlatformsFromIds(idsplat).Result;

            // Adds the data
            builder.Entity<Genre>().HasData(genres);
            builder.Entity<Company>().HasData(companies);
            builder.Entity<Platform>().HasData(platforms);

            
            builder.Entity<Game>().HasData(gamesList.Select(game => new
            {
                game.GameId,
                game.IgdbId,
                game.Name,
                game.Summary,
                game.IgdbRating,
                game.ImageUrl,
                game.Screenshots,
                game.VideoUrl,
                game.QvRating,
                game.ReleaseDate
            }).ToArray());

            // Many to many relationships:
            builder.Entity<GameGenre>().HasData(gamesList.SelectMany(game =>
            game.GameGenres.Select(gg => new
            {
                gg.IgdbId,
                gg.GenreId,
            })
                ).ToArray());

            var companyIds = companies.Select(c=> c.CompanyId).ToList();
            builder.Entity<GameCompany>().HasData(gamesList
                .SelectMany(game => game.GameCompanies
                    .Where(gg => companyIds.Contains(gg.CompanyId))
                    .Select(gg => new
                    {
                        gg.IgdbId,
                        gg.CompanyId,
                    }))
                .ToArray());

            var platformIds = platforms.Select(p => p.PlatformId).ToList();
            builder.Entity<GamePlatform>().HasData(gamesList
                .SelectMany(game => game.GamePlatforms
                    .Where(gg => platformIds.Contains(gg.PlatformId))
                    .Select(gg => new
                    {
                        gg.IgdbId,
                        gg.PlatformId,
                    }))
                .ToArray());
            Console.WriteLine("BLING BANG BANG BORN");
        }
        public DbSet<Game> Games { get; set; } = default!;
        public DbSet<Genre> Genres { get; set; } = default!;
        public DbSet<Platform> Platforms { get; set; } = default!;
        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<GameCompany> GameCompany { get; set; } = default!;
        public DbSet<GameGenre> GameGenre { get; set; } = default!;
        public DbSet<GamePlatform> GamePlatform { get; set; } = default!;

    }
}
