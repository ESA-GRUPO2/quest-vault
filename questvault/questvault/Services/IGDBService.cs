using IGDB;
using questvault.Models;

namespace questvault.Services
{
  /// <summary>
  /// Constructor that receives IGDB API credentials.
  /// </summary>
  /// <param name="igdbClientId">The IGDB API client ID.</param>
  /// <param name="igdbClientSecret">The IGDB API client secret.</param>
  public class IGDBService(string igdbClientId, string igdbClientSecret) : IServiceIGDB
  {

    private readonly IGDBClient _api = new(igdbClientId, igdbClientSecret);

    public async Task<IEnumerable<Game>> SearchGamesSteam(List<string> gameNames)
    {
      string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, cover.image_id, first_release_date, screenshots.image_id, videos.video_id, status;" +
          $"where name =(";
      foreach( string g in gameNames )
      {
        query += $" \"{g}\",";
      }
      query = query.TrimEnd(',') + ") &genres != null & cover.image_id != null & involved_companies != null & platforms != null & screenshots.image_id != null & videos.video_id != null & first_release_date != null; " +
          "sort total_rating_count desc; limit 80;";
      var games = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);
      return games
        .Where(g => g != null &&
                    g.FirstReleaseDate != null &&
                    g.Name != null &&
                    g.Rating != null &&
                    g.TotalRatingCount != null &&
                    g.Cover != null &&
                    g.Screenshots != null &&
                    g.Genres != null &&
                    g.InvolvedCompanies != null &&
                    g.Platforms != null
              )
        .Select(g => BuildGameFromIGDBGame(g));
    }

    public async Task<IEnumerable<Game>> SearchGames(string searchTerm)
    {

      string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, cover.image_id, first_release_date, screenshots.image_id, videos.video_id, status;" +
          $"where name ~ *\"{searchTerm}\"* & genres != null & cover.image_id != null & involved_companies != null & platforms != null & screenshots.image_id != null & videos.video_id != null & first_release_date != null;" +
          "sort total_rating_count desc;" +
          "limit 50;";
      var games = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

      return games.Select(g => BuildGameFromIGDBGame(g));
    }

    public async Task<IEnumerable<Game>> GetPopularGames(int limit)
    {
      string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, cover.image_id, first_release_date, screenshots.image_id, videos.video_id;" +
          "where name != null & genres != null & cover.image_id != null & involved_companies != null & platforms != null & first_release_date != null & screenshots.image_id != null & videos.video_id != null;" +
          "sort total_rating_count desc;" +
          $"limit {limit};";

      IGDB.Models.Game[] popularGames = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

      return popularGames.Select(g => BuildGameFromIGDBGame(g));
    }

    /// <summary>
    /// Builds a Game object from IGDB game data.
    /// </summary>
    /// <param name="igdbGame">The IGDB game data.</param>
    /// <returns>The constructed Game object.</returns>
    private static Game BuildGameFromIGDBGame(IGDB.Models.Game igdbGame)
    {
      string? url;
      try
      {
        url = "https://www.youtube.com/embed/" + igdbGame.Videos.Values.First().VideoId;
      }
      catch( NullReferenceException )
      {
        url = null;
      }
      var game = new Game
      {
        GameId = (long)igdbGame.Id,
        IgdbId = (long)igdbGame.Id,
        Name = igdbGame.Name,
        Summary = igdbGame.Summary,
        IgdbRating = (igdbGame.Rating.HasValue) ? (double)igdbGame.Rating : 0,
        TotalRatingCount = (igdbGame.TotalRatingCount.HasValue) ? igdbGame.TotalRatingCount : 0,
        ImageUrl = ImageHelper.GetImageUrl(imageId: igdbGame.Cover.Value.ImageId, size: ImageSize.CoverBig, retina: false),
        Screenshots = igdbGame.Screenshots.Values.Take(3).Select(s =>
            ImageHelper.GetImageUrl(imageId: s.ImageId, size: ImageSize.ScreenshotMed, retina: false)
          ).ToArray(),
        VideoUrl =  url,
        ReleaseDate = igdbGame.FirstReleaseDate.Value.Date,
        IsReleased = igdbGame.FirstReleaseDate.HasValue && igdbGame.FirstReleaseDate.Value.Date < DateTime.Today,
        GameGenres = igdbGame.Genres.Values.Select(genre => BuildGameGenreFromIGDBData(igdbGame, genre)).ToList(),
        GameCompanies = igdbGame.InvolvedCompanies.Values.Select(company => BuildGameCompanyFromIGDBData(igdbGame, company)).ToList(),
        GamePlatforms = igdbGame.Platforms.Values.Select(platform => BuildGamePlatformFromIGDBData(igdbGame, platform)).ToList()
      };

      return game;
    }

    /// <summary>
    /// Builds a GameGenre object from IGDB game genre data.
    /// </summary>
    /// <param name="igdbGame">The IGDB game data.</param>
    /// <param name="genreData">The IGDB game genre data.</param>
    /// <returns>The constructed GameGenre object.</returns>
    private static GameGenre BuildGameGenreFromIGDBData(IGDB.Models.Game igdbGame, IGDB.Models.Genre genreData)
    {
      return new GameGenre
      {
        IgdbId = (long) igdbGame.Id,
        IgdbGenreId = (long) genreData.Id,
        Genre = new Genre
        {
          GenreId = (long) genreData.Id,
          GenreName = genreData.Name
        }
      };
    }

    /// <summary>
    /// Builds a GameCompany object from IGDB game company data.
    /// </summary>
    /// <param name="igdbGame">The IGDB game data.</param>
    /// <param name="companyData">The IGDB game company data.</param>
    /// <returns>The constructed GameCompany object.</returns>
    private static GameCompany BuildGameCompanyFromIGDBData(IGDB.Models.Game igdbGame, IGDB.Models.InvolvedCompany companyData)
    {
      return new GameCompany
      {

        IgdbId = (long) igdbGame.Id,
        IgdbCompanyId = (long) companyData.Company.Id,
        Company = new Company
        {
          CompanyId = (long) companyData.Company.Id,

        }
      };
    }

    /// <summary>
    /// Builds a GamePlatform object from IGDB game platform data.
    /// </summary>
    /// <param name="igdbGame">The IGDB game data.</param>
    /// <param name="platformData">The IGDB game platform data.</param>
    /// <returns>The constructed GamePlatform object.</returns>
    private static GamePlatform BuildGamePlatformFromIGDBData(IGDB.Models.Game igdbGame, IGDB.Models.Platform platformData)
    {
      return new GamePlatform
      {
        IgdbId = (long) igdbGame.Id,
        IgdbPlatformId = (long) platformData.Id,
        Platform = new Platform
        {
          PlatformId = (long) platformData.Id
        }
      };
    }
    public async Task<IEnumerable<Genre>> GetGenres()
    {
      var endpoint = IGDBClient.Endpoints.Genres;
      var genres = await _api.QueryAsync<IGDB.Models.Genre>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(genres);
      return genres.Select(genre => new Genre
      {
        GenreId = (long) genre.Id,
        IgdbGenreId = (long) genre.Id,
        GenreName = genre.Name
      });
    }
    public async Task<IEnumerable<Company>> GetCompanies()
    {
      var endpoint = IGDBClient.Endpoints.Companies;
      var companies = await _api.QueryAsync<IGDB.Models.Company>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(companies);
      return companies.Select(c => new Company
      {
        CompanyId = (long) c.Id,
        IgdbCompanyId = (long) c.Id,
        CompanyName = c.Name
      });
    }
    public async Task<IEnumerable<Company>> GetCompaniesFromIds(List<long> ids)
    {
      var endpoint = IGDBClient.Endpoints.Companies;
      string query = $"fields id, name; where id = ({string.Join(",", ids)});";
      var companies = await _api.QueryAsync<IGDB.Models.Company>(endpoint, query);
      Console.WriteLine(companies);
      return companies.Select(c => new Company
      {
        CompanyId = (long) c.Id,
        IgdbCompanyId = (long) c.Id,
        CompanyName = c.Name,

      });
    }
    public async Task<IEnumerable<Genre>> GetGenresFromIds(List<long> ids)
    {
      var endpoint = IGDBClient.Endpoints.Genres;
      string query = $"fields id, name; where id = ({string.Join(",", ids)});";
      var genres = await _api.QueryAsync<IGDB.Models.Genre>(endpoint, query);
      Console.WriteLine(genres);
      return genres.Select(c => new Genre
      {
        GenreId = (long) c.Id,
        IgdbGenreId = (long) c.Id,
        GenreName = c.Name,

      });
    }
    public async Task<IEnumerable<Platform>> GetPlatforms()
    {
      var endpoint = IGDBClient.Endpoints.Platforms;
      var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(platforms);
      return platforms.Select(p => new Platform
      {
        PlatformId = (long) p.Id,
        IgdbPlatformId = (long) p.Id,
        PlatformName = p.Name
      });
    }
    public async Task<IEnumerable<Platform>> GetPlatformsFromIds(List<long> ids)
    {
      var endpoint = IGDBClient.Endpoints.Platforms;
      string query = $"fields id, name; where id = ({string.Join(",", ids)});";
      var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, query);
      Console.WriteLine(platforms);
      return platforms.Select(p => new Platform
      {
        PlatformId = (long) p.Id,
        IgdbPlatformId = (long) p.Id,
        PlatformName = p.Name
      });
    }

    /// <summary>
    /// This method retrieves the Steam URL for a game based on the IGDB game ID.
    /// </summary>
    /// <param name="igdbGameId">The IGDB game ID.</param>
    /// <returns>The Steam URL for the game.</returns>
    public async Task<string> GetSteamUrl(long? igdbGameId)
    {
      if( igdbGameId == null ) return string.Empty;
      var query = $"fields url; where game = {igdbGameId} & category = 13;";
      var result = await _api.QueryAsync<IGDB.Models.Website>(IGDBClient.Endpoints.Websites, query);
      var url = result.Select(r => r.Url).FirstOrDefault();
      return url ?? string.Empty;
    }
  }
}
