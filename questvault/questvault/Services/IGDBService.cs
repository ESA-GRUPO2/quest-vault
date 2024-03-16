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
        
      return games.Select(game => new Game
      {
        GameId = (int)game.Id,
        Name = game.Name,
        Summary = game.Summary,
        IgdbRating = game.Rating.Value,
        imageUrl = ImageHelper.GetImageUrl(imageId: game.Artworks.Values.First().ImageId, size: ImageSize.CoverBig, retina: true),

      });
    }

    public async Task<IEnumerable<Game>> GetPopularGames(int limit)
    {
      string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, artworks.image_id;" +
          "where name != null & genres != null & rating != null & total_rating_count != null & artworks.image_id != null & involved_companies != null & platforms != null;" +
          "sort total_rating_count desc;" +
          $"limit {limit};";

      var popularGames = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

      return popularGames.Select(game => new Game
      {
        GameId = (long)game.Id,
        Name = game.Name,
        Summary = game.Summary,
        IgdbRating = (double)game.Rating,
        imageUrl = ImageHelper.GetImageUrl(imageId: game.Artworks.Values.First().ImageId, size: ImageSize.CoverBig, retina: true),
        GameGenres = game.Genres.Values.Select(genre => new GameGenre
        {
            GameId = (long)game.Id,
            GenreId = (long)genre.Id,
            Genre = new Genre
            {
                GenreId = (long)genre.Id,
                GenreName = genre.Name,
                GameGenres = new List<GameGenre>
                {
                    new GameGenre
                    {
                        GameId = (long)game.Id,
                        GenreId = (long)genre.Id
                    }

                }
            }

        }).ToList(),
         
          GameCompanies = game.InvolvedCompanies.Values.Select(comp => new GameCompany
          {
              GameId = (long)game.Id,
              CompanyId = (long)comp.Company.Id,
              Company = new Company
              {
                  CompanyId = (long)comp.Company.Id,
                  GameCompanies = new List<GameCompany>
                  {
                      new GameCompany
                      {
                          GameId = (long)game.Id,
                          CompanyId = (long)comp.Company.Id,
                      }
                  }
              }
          }).ToList(),

      });
    }


    public async Task<IEnumerable<Models.Genre>> GetGenres()
    {
      var endpoint = IGDBClient.Endpoints.Genres;
      var genres = await _api.QueryAsync<IGDB.Models.Genre>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(genres);
      return genres.Select(genre => new Models.Genre
      {
        GenreId = (long)genre.Id,
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
        CompanyId = (long)(c.Id),
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
        CompanyId = (long)c.Id,
        CompanyName = c.Name
      });
    }

        //public async Task<IEnumerable<Platform>> GetAllPlatforms()
        //{
        //    var allPlatforms = new List<Platform>();
        //    var pageSize = 500; // Tamanho da página, ajuste conforme necessário
        //    var currentPage = 0;
        //    var totalPages = 0; // Defina como 1 inicialmente para entrar no loop

        //    while (currentPage <= totalPages)
        //    {
        //        var endpoint = IGDBClient.Endpoints.Platforms;
        //        var queryString = $"fields id,name; limit {pageSize}; offset {currentPage * pageSize};";
        //        var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, queryString);

        //        allPlatforms.AddRange(platforms.Select(p => new Platform
        //        {
        //            PlatformId = (long)p.Id,
        //            PlatformName = p.Name
        //        }));

        //        // Verificar o cabeçalho de resposta para determinar o número total de páginas
        //        totalPages = (int)Math.Floor((double)allPlatforms.Count() / pageSize); // Implemente essa função de acordo com a resposta real da API

        //        currentPage++;
        //    }

        //    return allPlatforms;
        //}


    public async Task<IEnumerable<Platform>> GetPlatforms()
    {
      var endpoint = IGDBClient.Endpoints.Platforms;
      var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, "fields id,name; limit 500;");
      Console.WriteLine(platforms);
      return platforms.Select(p => new Platform
      {
        PlatformId = (long)p.Id,
        PlatformName = p.Name
      });
    }


  }
}
