using IGDB;
using questvault.Models;
using System.Collections.Generic;

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

            string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, cover.image_id, first_release_date, screenshots.image_id, videos.video_id, status;" +
                $"where name ~ *\"{searchTerm}\"* & genres != null & cover.image_id != null & involved_companies != null & platforms != null & screenshots.image_id != null & videos.video_id != null & first_release_date != null;" +
                "sort total_rating_count desc;" +
                "limit 50;";
            var games = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);
            
            return games.Select(game => BuildGameFromIGDBGame(game));
        }

        //public async Task<IEnumerable<Game>> GetPopularGames(int limit)
        //{
        //    string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, artworks.image_id;" +
        //        "where name != null & genres != null & rating != null & total_rating_count != null & artworks.image_id != null & involved_companies != null & platforms != null;" +
        //        "sort total_rating_count desc;" +
        //        $"limit {limit};";

        //    var popularGames = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

        //    return popularGames.Select(game => new Game
        //    {
        //        GameId = (long)game.Id,
        //        Name = game.Name,
        //        Summary = game.Summary,
        //        IgdbRating = (double)game.Rating,
        //        imageUrl = ImageHelper.GetImageUrl(imageId: game.Artworks.Values.First().ImageId, size: ImageSize.CoverBig, retina: true),
        //        GameGenres = game.Genres.Values.Select(genre => new GameGenre
        //        {
        //            GameId = (long)game.Id,
        //            GenreId = (long)genre.Id,
        //            Genre = new Genre
        //            {
        //                GenreId = (long)genre.Id,
        //                GenreName = genre.Name,
        //                GameGenres = new List<GameGenre>
        //        {
        //            new GameGenre
        //            {
        //                GameId = (long)game.Id,
        //                GenreId = (long)genre.Id
        //            }

        //        }
        //            }

        //        }).ToList(),

        //        GameCompanies = game.InvolvedCompanies.Values.Select(comp => new GameCompany
        //        {
        //            GameId = (long)game.Id,
        //            CompanyId = (long)comp.Company.Id,
        //            Company = new Company
        //            {
        //                CompanyId = (long)comp.Company.Id,
        //                GameCompanies = new List<GameCompany>
        //          {
        //              new GameCompany
        //              {
        //                  GameId = (long)game.Id,
        //                  CompanyId = (long)comp.Company.Id,
        //              }
        //          }
        //            }
        //        }).ToList(),
        //        GamePlatforms = game.Platforms.Values.Select(p => new GamePlatform
        //        {
        //            GameId = (long)game.Id,
        //            PlatformId = (long)p.Id,
        //            Platform = new Platform
        //            {
        //                PlatformId = (long)p.Id,
        //                GamePlatforms = new List<GamePlatform>
        //          {
        //              new GamePlatform
        //              {
        //                  GameId = (long)game.Id,
        //                  PlatformId = (long)p.Id,
        //              }
        //          }
        //            }
        //        }
        //        ).ToList(),
        //    });
        //}

        public async Task<IEnumerable<Game>> GetPopularGames(int limit)
        {
            string query = "fields id, name, genres.name, involved_companies.company, platforms.name, rating, total_rating_count, summary, cover.image_id, first_release_date, screenshots.image_id, videos.video_id;" +
                "where name != null & genres != null & cover.image_id != null & involved_companies != null & platforms != null & first_release_date != null & screenshots.image_id != null & videos.video_id != null;" +
                "sort total_rating_count desc;" +
                $"limit {limit};";
            
            var popularGames = await _api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query);

            return popularGames.Select(game => BuildGameFromIGDBGame(game));
        }
        
        /// <summary>
        /// Builds a Game object from IGDB game data.
        /// </summary>
        /// <param name="igdbGame">The IGDB game data.</param>
        /// <returns>The constructed Game object.</returns>
        private static Game BuildGameFromIGDBGame(IGDB.Models.Game igdbGame)
        {
            //Console.WriteLine(igdbGame.Rating.HasValue ? "O rating não é nulo" : "O rating é nulo");

            var game = new Game
            {
                GameId = (long)igdbGame.Id,
                IgdbId = (long)igdbGame.Id,
                Name = igdbGame.Name,
                Summary = igdbGame.Summary,
                IgdbRating = (igdbGame.Rating.HasValue) ? (double)igdbGame.Rating : 0,
                ImageUrl = ImageHelper.GetImageUrl(imageId: igdbGame.Cover.Value.ImageId, size: ImageSize.CoverBig, retina: false),
                Screenshots = igdbGame.Screenshots.Values.Take(3).Select(s =>
                    ImageHelper.GetImageUrl(imageId: s.ImageId, size: ImageSize.ScreenshotMed, retina: false)
                ).ToArray(),
                VideoUrl = "https://www.youtube.com/embed/" + igdbGame.Videos.Values.First().VideoId,
                ReleaseDate = igdbGame.FirstReleaseDate.Value.Date,
                IsReleased = (igdbGame.FirstReleaseDate.HasValue && igdbGame.FirstReleaseDate.Value.Date < DateTime.Today) ? true : false,
                //igdbGame.FirstReleaseDate.Value.ToString("MMMM") + " " + 
                //    igdbGame.FirstReleaseDate.Value.Day + ", " + 
                //    igdbGame.FirstReleaseDate.Value.Year,
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
                IgdbId = (long)igdbGame.Id,
                IgdbGenreId = (long)genreData.Id,
                Genre = new Genre
                {
                    GenreId = (long)genreData.Id,
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

                IgdbId = (long)igdbGame.Id,
                IgdbCompanyId = (long)companyData.Company.Id,
                Company = new Company
                {
                    CompanyId = (long)companyData.Company.Id,
                    
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
                IgdbId = (long)igdbGame.Id,
                IgdbPlatformId = (long)platformData.Id,
                Platform = new Platform
                {
                    PlatformId = (long)platformData.Id
                }
            };
        }

        public async Task<IEnumerable<Models.Genre>> GetGenres()
        {
            var endpoint = IGDBClient.Endpoints.Genres;
            var genres = await _api.QueryAsync<IGDB.Models.Genre>(endpoint, "fields id,name; limit 500;");
            Console.WriteLine(genres);
            return genres.Select(genre => new Models.Genre
            {
                GenreId = (long)genre.Id,
                IgdbGenreId = (long)genre.Id,
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
                IgdbCompanyId = (long)c.Id,
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
                IgdbCompanyId= (long)c.Id,
                CompanyName = c.Name
            });
        }
        public async Task<IEnumerable<Platform>> GetPlatforms()
        {
            var endpoint = IGDBClient.Endpoints.Platforms;
            var platforms = await _api.QueryAsync<IGDB.Models.Platform>(endpoint, "fields id,name; limit 500;");
            Console.WriteLine(platforms);
            return platforms.Select(p => new Platform
            {
                PlatformId = (long)p.Id,
                IgdbPlatformId  = (long)p.Id,
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
                PlatformId = (long)p.Id,
                IgdbPlatformId= (long)p.Id,
                PlatformName = p.Name
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

    }
}
