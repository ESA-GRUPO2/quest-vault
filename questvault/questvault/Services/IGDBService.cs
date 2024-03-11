using IGDB.Models;
using IGDB;
using questvault.Models;
using System.Security.Permissions;
using Newtonsoft.Json;
using questvault.Data;

namespace questvault.Services
{
    public class IGDBService
    {

        private readonly IGDBClient _api;
        public IGDBService(string apiKey, string apiSecret)
        {
            _api = new IGDBClient(apiKey, apiSecret);
        }

        public async Task<IEnumerable<Games>> SearchGames(string searchTerm)
        {
            string query = $"fields id,name, genres; search *\"{searchTerm}\"*; limit 5;";
            var games = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query);

            return games.Select(game => new Games
            {
                GameID = (int)game.Id,
                Name = game.Name
            });
        }

        //public async Task<IEnumerable<Games>> GetPopularGames(int limit)
        //{
        //    string query = "fields id,name,genres.name,rating,total_rating_count, summary;" +
        //        "where name != null & genres != null & rating != null & total_rating_count != null;" +
        //        "sort total_rating_count desc;" +
        //        $"limit {limit};";

        //    var popularGames = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query);

        //    return popularGames.Select(game => new Games
        //        {
        //            GameID = (int)game.Id,
        //            Name = game.Name,
        //            Summary = game.Summary,
        //            Genres = game.Genres.Values.Select(genre => new Genres
        //            {
        //                GenreID = (int)genre.Id,
        //                GenreName = genre.Name,
        //                // Outros campos do gênero
        //            }).ToList(),
        //            Rating = (double)game.Rating
        //        });
        //}

        public async Task<IEnumerable<Games>> GetPopularGames(int limit)
        {
            string query = "fields id,name,genres.name,rating,total_rating_count, summary;" +
                "where name != null & genres != null & rating != null & total_rating_count != null;" +
                "sort total_rating_count desc;" +
                $"limit {limit};";

            var popularGames = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query);

            return popularGames.Select(game => new Games
            {
                GameID = (int)game.Id,
                Name = game.Name,
                Summary = game.Summary,
                Rating = (double)game.Rating,
                GamesGenres = game.Genres.Values.Select(genre => new GamesGenres
                {
                    GamesID = (int)game.Id,
                    GenresID = (int)genre.Id,
                    Genre = new Genres
                    {
                        GenreID = (int)genre.Id,
                        GenreName = genre.Name,
                        GamesGenres = new List<GamesGenres>
                        {
                            new GamesGenres
                            {
                                GamesID = (int)game.Id,
                                GenresID = (int)genre.Id
                            }
                        }
                    },
                }).ToList()
            });
        }


        public async Task<IEnumerable<Genres>> GetGenres()
        {
            var endpoint = IGDBClient.Endpoints.Genres;
            var genres = await _api.QueryAsync<Genre>(endpoint, "fields id,name; limit 500;");
            Console.WriteLine(genres);
            return genres.Select(genre => new Genres
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
