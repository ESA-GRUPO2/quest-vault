using IGDB;
using questvault.Models;

namespace questvault.Services
{
  public class IGDBService : IServiceIGDB
  {

    private readonly IGDBClient _api;

    /// <summary>
    /// Constructor that receives IGDB API credentials.
    /// </summary>
    /// <param name="igdbClientId">The IGDB API client ID.</param>
    /// <param name="igdbClientSecret">The IGDB API client secret.</param>
    public IGDBService(string igdbClientId, string igdbClientSecret)
    {
      _api = new IGDBClient(igdbClientId, igdbClientSecret);
    }

    public async Task<IEnumerable<Models.Game>> SearchGames(string searchTerm)
    {

      string query = $"fields id,name, genres; search *\"{searchTerm}\"*; limit 5;";
      string query2 = "fields id,name,genres.name,rating,total_rating_count, summary, artworks.image_id;" +
          $"where name ~ *\"{searchTerm}\"* & genres != null & rating != null & total_rating_count != null & summary != null & artworks.image_id != null;" +
          "sort total_rating_count desc;" +
          "limit 5;";
      var games = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query2);

      return games.Select(game => new Models.Game
      {
        GameID = (int)game.Id,
        Name = game.Name,
        Summary = game.Summary,
        IgdbRating = game.Rating.Value,
        imageUrl = ImageHelper.GetImageUrl(imageId: game.Artworks.Values.First().ImageId, size: ImageSize.CoverBig, retina: true)
      });
    }

    public async Task<IEnumerable<Models.Game>> GetPopularGames(int limit)
    {
      string query = "fields id,name,genres.name,rating,total_rating_count, summary; artworks.image_id;" +
          "where name != null & genres != null & rating != null & total_rating_count != null;" +
          "sort total_rating_count desc;" +
          $"limit {limit};";

      var popularGames = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

      return popularGames.Select(game => new Models.Game
      {
        GameID = (int)game.Id,
        Name = game.Name,
        Summary = game.Summary,
        IgdbRating = (double)game.Rating,
        imageUrl = ImageHelper.GetImageUrl(imageId: game.Artworks.Values.First().ImageId, size: ImageSize.CoverBig, retina: true),
        GamesGenres = game.Genres.Values.Select(genre => new GameGenre
        {
          GamesID = (int)game.Id,
          GenresID = (int)genre.Id,
          Genre = new Models.Genre
          {
            GenreID = (int)genre.Id,
            GenreName = genre.Name,
            GamesGenres = new List<GameGenre>
                        {
                            new GameGenre
                            {
                                GamesID = (int)game.Id,
                                GenresID = (int)genre.Id
                            }
                        }
          },
        }).ToList()
        //,
        //GamePlatform = game.Platforms.Values.Select(pl => new GamePlatform
        //{
        //    GameID = (int)game.Id,
        //    PlatformID = (int)pl.Id,
        //    Platform = new Models.Platform
        //    {
        //        PlatformID = (int)pl.Id,
        //        PlatformName = pl.Name,
        //        GamePlatform = new List<GamePlatform>
        //        {
        //            new GamePlatform { 
        //                GameID = (int)game.Id,
        //                PlatformID = (int)pl.Id
        //            }
        //        }
        //    }
        //}).ToList()
      });
    }


    public async Task<IEnumerable<Models.Genre>> GetGenres()
    {
      var endpoint = IGDBClient.Endpoints.Genres;
      var genres = await _api.QueryAsync<IGDB.Models.Genre>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(genres);
      return genres.Select(genre => new Models.Genre
      {
        GenreID = (int)genre.Id,
        GenreName = genre.Name
      });
    }

    public async Task<IEnumerable<Models.Company>> GetCompanies()
    {
      var endpoint = IGDBClient.Endpoints.Companies;
      var companies = await _api.QueryAsync<IGDB.Models.Company>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(companies);
      return companies.Select(c => new Models.Company
      {
        CompanyID = (int)(c.Id),
        CompanyName = c.Name
      });
    }

    public async Task<IEnumerable<Models.Platform>> GetPlatforms()
    {
      var endpoint = IGDBClient.Endpoints.Platforms;
      var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(platforms);
      return platforms.Select(p => new Models.Platform
      {
        PlatformID = (int)(p.Id),
        PlatformName = p.Name
      });
    }


  }
}
