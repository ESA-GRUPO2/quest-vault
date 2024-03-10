using IGDB.Models;
using IGDB;
using questvault.Models;

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

    }
}
