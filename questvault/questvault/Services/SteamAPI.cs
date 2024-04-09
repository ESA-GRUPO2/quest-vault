using Microsoft.IdentityModel.Tokens;
using questvault.Models;
using System.Text.Json;

namespace questvault.Services
{
  public class SteamAPI(string steamApiKey, IServiceIGDB igdbService)
  {
    private readonly HttpClient client = new();

    public async Task<List<GameInfo>> GetUserLibrary(string steamId)
    {
      string url = $"http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/" +
        $"?key={steamApiKey}" +
        $"&steamid={steamId}" +
        "&include_appinfo=true" +
        "&include_played_free_games=true" +
        "&include_free_sub=true" +
        "&skip_unvetted_apps=true" +
        "&include_extended_appinfo=true";

      HttpResponseMessage response = await client.GetAsync(url);

      if( response.IsSuccessStatusCode )
      {
        string json = await response.Content.ReadAsStringAsync();
        if( json == null ) return [];

        JsonDocument doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
        List<GameInfo> jsonGames = [];
        if( root.TryGetProperty("response", out JsonElement responseElement) && responseElement.TryGetProperty("games", out JsonElement gamesElement) )
        {
          foreach( JsonElement game in gamesElement.EnumerateArray() )
          {
            if( game.TryGetProperty("name", out JsonElement nameElement) &&
                game.TryGetProperty("playtime_forever", out JsonElement playtimeElement) &&
                game.TryGetProperty("appid", out JsonElement appidElement) &&
                nameElement.ValueKind == JsonValueKind.String &&
                !nameElement.GetString().IsNullOrEmpty()
              )
            {
              jsonGames.Add(new() { Appid = appidElement.GetInt32(), Name = nameElement.GetString(), Playtime = playtimeElement.GetInt32() });
            }
          }
        }
        doc.Dispose();
        return jsonGames;
      }
      else
      {
        await Console.Out.WriteLineAsync(response.StatusCode.ToString());
        // Handle unsuccessful API request
      }
      return [];
    }

    public async Task<List<GameLog>> GetGamesFromIGDB(List<GameInfo> jsonGames)
    {
      int index = 0;
      int batchSize = 50; //the safe ammount that we can search in IGDB at once is 80
      List<GameLog> gameLogs = [];
      while( index < jsonGames.Count )
      {
        if( index + batchSize > jsonGames.Count ) batchSize = jsonGames.Count - index; // para nao perdermos os ultimos
        List<GameInfo> batch = jsonGames.Skip(index).Take(batchSize).ToList(); //fazer o batch
        await Console.Out.WriteLineAsync("batch: " + batch.Count);
        IEnumerable<Game> games = await igdbService.SearchGamesSteam(batch.Select(x=>x.Name).ToList()); //pesquisar na igdb o batch
        await Console.Out.WriteLineAsync("from igdb: " + games.Count());

        foreach( Game g in games ) // percorrer os jogos encontrados na igdb para criar os GameLogs
        {
          GameInfo? current = batch.FirstOrDefault(jg => jg.Name.Equals(g.Name));
          GameLog gl = new()
          {
            Game = g,
            IgdbId = g.IgdbId,
            HoursPlayed = current?.Playtime ?? 0,
            Ownage = current?.Playtime>0 ? OwnageStatus.Playing : OwnageStatus.Backlogged,
            Status = GameStatus.Shelved
          };
          gameLogs.Add(gl);
        }

        index += batchSize;
      }

      Console.WriteLine($"{jsonGames.Count - gameLogs.Count} games weren't found in IGDB.");
      return gameLogs;
    }

    public class GameInfo
    {
      public int Appid { get; set; }
      public string Name { get; set; }
      public int Playtime { get; set; }
    }

  }
}