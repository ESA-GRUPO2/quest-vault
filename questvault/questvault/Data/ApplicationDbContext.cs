using IGDB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using questvault.Models;
using questvault.Services;
using System.Reflection.Emit;

namespace questvault.Data
{
  public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
  {
    public DbSet<TwoFactorAuthenticationTokens> EmailTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<TwoFactorAuthenticationTokens>().HasKey(t => t.UserId);


            //POPULATE

            
            var _igdbService = new IGDBService("uzhx4rrftyohllg1mrpy3ajo7090q5", "7rvcth933kxra92ddery5qn3jxwap7");
            //var companies = _igdbService.GetCompanies().Result;
            //var genres = _igdbService.GetGenres().Result;
            //var top500Games = _igdbService.GetPopularGames(500).Result;

            //builder.Entity<Games>().HasData(top500Games);

            //builder.Entity<Genres>().HasData(genres);
            ////builder.Entity<Models.Company>().HasData(companies);

            var platforms = _igdbService.GetPlatforms().Result;
            var genres = _igdbService.GetGenres().Result;
            var topGames = _igdbService.GetPopularGames(50).Result;

            //builder.Entity<Models.Platform>().HasData(platforms);
            builder.Entity<Models.Genre>().HasData(genres);

            // Adiciona apenas os jogos, pois os gêneros são adicionados separadamente
            builder.Entity<Models.Game>().HasData(topGames.Select(game => new
            {
                GameID = game.GameID,
                Name = game.Name,
                Summary = game.Summary,
                Rating = game.Rating
            }).ToArray());

            // Adiciona os relacionamentos muitos-para-muitos
            builder.Entity<GameGenre>().HasData(topGames.SelectMany(game =>
                game.GamesGenres.Select(gg => new
                {
                    GamesID = gg.GamesID,
                    GenresID = gg.GenresID
                })
            ).ToArray());

            //builder.Entity<GamePlatform>().HasData(topGames.SelectMany(game =>
            //    game.GamePlatform.Select(gg => new
            //    {
            //        GamesID = gg.GameID,
            //        PlatformID = gg.PlatformID
            //    })
            //).ToArray());
        }
      public DbSet<Models.Genre> Genres { get; set; } = default!;
      public DbSet<questvault.Models.Platform> Platform { get; set; } = default!;
      public DbSet<questvault.Models.Company> Company { get; set; } = default!;
      public DbSet<Models.Game> Games { get; set; } = default!;
  }
}
