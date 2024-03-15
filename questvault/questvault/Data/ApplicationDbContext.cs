
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using questvault.Models;
using questvault.Services;
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


            builder.Entity<GameGenres>()
         .HasKey(gg => new { gg.GameId, gg.GenreId });

            builder.Entity<GameGenres>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GameId);

            builder.Entity<GameGenres>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId);

            //// Get data from IGDB service
            var genres = igdbService.GetGenres().Result;
            //var platforms = igdbService.GetPlatforms().Result;
            //var companies = igdbService.GetCompanies().Result;
            var gamesList = igdbService.GetPopularGames(10).Result;


            var gamesWithGenres = gamesList.SelectMany(game => game.Genres.Select(genre => new GameGenres
            {
                GameId = game.GameId,
                GenreId = genre.GenreId
            }));
            builder.Entity<Genre>().HasData(genres);

            builder.Entity<GameGenres>().HasData(gamesWithGenres);
            //// Add data to the database
            //builder.Entity<Platform>().HasData(platforms);
            //builder.Entity<Company>().HasData(companies);
            builder.Entity<Game>().HasData(gamesList);

        }
      public DbSet<Game> Games { get; set; } = default!;
      public DbSet<Genre> Genres { get; set; } = default!;
      public DbSet<Platform> Platforms { get; set; } = default!;
      public DbSet<Company> Companies { get; set; } = default!;
  }
}
