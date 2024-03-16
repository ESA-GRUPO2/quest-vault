
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

        


            //// Get data from IGDB service
            var genres = igdbService.GetGenres().Result;
            //var platforms = igdbService.GetPlatforms().Result;
           
            var gamesList = igdbService.GetPopularGames(10).Result;
            var idscomp = gamesList.SelectMany(game =>
                game.GameCompanies.Select(comp => comp.CompanyId)).Distinct().ToList();

            var companies = igdbService.GetCompaniesFromIds(idscomp).Result;


            builder.Entity<Genre>().HasData(genres);
            builder.Entity<Company>().HasData(companies);

            // Adiciona apenas os jogos, pois os gêneros são adicionados separadamente
            builder.Entity<Game>().HasData(gamesList.Select(game => new 
            {
                GameId = game.GameId,
                Name = game.Name,
                Summary = game.Summary,
                IgdbRating = game.IgdbRating,
                QvRating = game.QvRating,
                imageUrl = game.imageUrl
            }).ToArray());

            builder.Entity<GameGenre>().HasData(gamesList.SelectMany( game => 
            game.GameGenres.Select( gg => new
            {
                GameId = gg.GameId,
                GenreId = gg.GenreId
            })
                ).ToArray());
            
            
            builder.Entity<GameCompany>().HasData(gamesList.SelectMany(game =>
            game.GameCompanies.Select(gg => new
            {
                GameId = gg.GameId,
                CompanyId = gg.CompanyId
            })).ToArray());


            //// Add data to the database
            //builder.Entity<Platform>().HasData(platforms);
            //builder.Entity<Game>().HasData(gamesList);

        }
        public DbSet<Game> Games { get; set; } = default!;
      public DbSet<Genre> Genres { get; set; } = default!;
      public DbSet<Platform> Platforms { get; set; } = default!;
      public DbSet<Company> Companies { get; set; } = default!;
  }
}
