using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using questvault.Data;
using questvault.Models;

namespace questvault.Services
{
  public class BackgroundImportLibrary(
      IServiceProvider serviceProvider
    ) : IHostedService, IDisposable
  {
    private Timer? timer;
    private Dictionary<string, string> queue = [];
    private SteamAPI? steamAPI = null;
    private ApplicationDbContext? context = null;
    private IServiceIGDB? igdbService = null;
    private bool mutex = true;

    private static int SaveChanges(ApplicationDbContext context)
    {
      try
      {
        return context.SaveChanges();
      }
      catch( Exception e )
      {
        if( e is DbUpdateConcurrencyException || e is DbUpdateException )
        {
          Console.Out.WriteLine("Data already in the db");
          return 0;
        }
        else
        {
          Console.Out.WriteLine($"Unexpected exception: {e.Message}");
          return -1;
        }
      }
    }

    private void ImportLibraryAsync(KeyValuePair<string, string> pair)
    {
      if( steamAPI == null || context == null || igdbService == null ) return;
      string userId = pair.Key;
      string steamId = pair.Value;
      Console.WriteLine($"queue size: {queue.Count}");
      Console.WriteLine($"[{userId}, {steamId}]");
      if( userId == null || steamId == null ) return;
      var watch = System.Diagnostics.Stopwatch.StartNew();
      List<SteamAPI.GameInfo> gamesInfoInSteamLibrary = [];
      List<GameLog> finalGameLogs = [];
      foreach( SteamAPI.GameInfo gameInfo in steamAPI.GetUserLibrary(steamId).Result )
      {
        Game? game = context.Games.FirstOrDefault(g => g.Name != null && g.Name.Equals(gameInfo.Name));
        if( game != null )
        {
          finalGameLogs.Add(new()
          {
            Game = game,
            GameId = game.GameId,
            IgdbId = game.IgdbId,
            HoursPlayed = gameInfo?.Playtime ?? 0,
            Status = gameInfo?.Playtime > 0 ? GameStatus.Playing : GameStatus.Backlogged,
            UserId = userId
          });
        }
        else gamesInfoInSteamLibrary.Add(gameInfo);
      }

      finalGameLogs.AddRange(steamAPI.GetGamesFromIGDB(gamesInfoInSteamLibrary).Result);

      // udpating/adding games in db
      {
        var games = finalGameLogs.Select(g => g.Game).Where(g => g != null).Cast<Game>();

        var companyIds = games.SelectMany (g => g.GameCompanies != null ? g.GameCompanies.Where(c=>c!=null).Select(c => c.IgdbCompanyId) : []).Distinct().ToList();
        var platformIds = games.SelectMany(g => g.GamePlatforms!=null ? g.GamePlatforms.Select(p => p.IgdbPlatformId) : []).Distinct().ToList();
        var genresIds = games.SelectMany(g => g.GameGenres!=null ? g.GameGenres.Select(gg => gg.IgdbGenreId) : []).Distinct().ToList();

        var existingCompanies = context.Companies.Where(c => companyIds.Contains(c.IgdbCompanyId)).ToList();
        var existingPlatforms = context.Platforms.Where(p => platformIds.Contains(p.IgdbPlatformId)).ToList();
        var existingGenres = context.Genres.Where(g => genresIds.Contains(g.IgdbGenreId)).ToList();

        bool allCompaniesExist = existingCompanies.Count == companyIds.Count;
        bool allPlatformsExist = existingPlatforms.Count == platformIds.Count;
        bool allGenresExist = existingGenres.Count == genresIds.Count;

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allCompaniesExist )
            {
              var missingCompanies = igdbService.GetCompaniesFromIds(
                companyIds.Except(existingCompanies.Select(c => c.IgdbCompanyId)).ToList()).Result;

              foreach( var company in missingCompanies )
              {
                var existingCompany = context.Companies.FirstOrDefault(g => g.IgdbCompanyId == company.IgdbCompanyId);
                if( existingCompany == null )
                {
                  var newComp = new Company
                  {
                    IgdbCompanyId = company.IgdbCompanyId,
                    CompanyName = company.CompanyName,
                  };
                  context.Companies.Add(newComp);

                  existingCompanies.Add(newComp);
                  SaveChanges(context);
                }
              }
            }
            transaction.CommitAsync();
          }
          catch( Exception ex )
          {
            transaction.RollbackAsync();
            return;
          }

        }

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allPlatformsExist )
            {
              var missingPlatforms = igdbService.GetPlatformsFromIds(
                  platformIds.Except(existingPlatforms.Select(c => c.IgdbPlatformId)).ToList()).Result;

              foreach( var platform in missingPlatforms )
              {
                var existingPlatform = context.Platforms.FirstOrDefaultAsync(g => g.IgdbPlatformId == platform.IgdbPlatformId);
                if( existingPlatform == null )
                {
                  var newPlat = new Models.Platform
                  {
                    IgdbPlatformId = platform.IgdbPlatformId,
                    PlatformName = platform.PlatformName,
                  };
                  context.Platforms.AddAsync(newPlat);
                  existingPlatforms.Add(newPlat);
                  SaveChanges(context);
                }
              }
            }

            // Commit da transação se tudo correr bem
            transaction.CommitAsync();
          }
          catch( Exception ex )
          {
            Console.Out.WriteLineAsync(ex.Message);
            // Rollback da transação em caso de erro
            transaction.RollbackAsync();
            // Tratar ou registrar o erro conforme necessário
            return;
          }
        }

        using( var transaction = context.Database.BeginTransaction() )
        {
          try
          {
            if( !allGenresExist )
            {
              var missingGenres = igdbService.GetGenresFromIds(
                genresIds.Except(existingGenres.Select(gg => gg.IgdbGenreId)).ToList()).Result;

              foreach( var genre in missingGenres )
              {
                var existingGenre = context.Genres.FirstOrDefaultAsync(g => g.IgdbGenreId == genre.IgdbGenreId);
                if( existingGenre == null )
                {
                  var newGenre = new Genre
                  {
                    IgdbGenreId = genre.IgdbGenreId,
                    GenreName = genre.GenreName
                  };
                  context.Genres.AddAsync(newGenre);
                  existingGenres.Add(newGenre);
                  SaveChanges(context);
                }
              }

            }
            transaction.CommitAsync();
          }
          catch( Exception ex )
          {
            transaction.RollbackAsync();
            return;
          }
        }

        foreach( var game in games )
        {
          var existingGame = context.Games.FirstOrDefaultAsync(g => g.IgdbId == game.IgdbId);

          if( existingGame == null )
          {
            var newGame = new Game
            {
              IgdbId = game.IgdbId,
              Name = game.Name,
              Summary = game.Summary,
              IgdbRating = game.IgdbRating,
              TotalRatingCount = game.TotalRatingCount,
              ImageUrl = game.ImageUrl,
              Screenshots = game.Screenshots,
              VideoUrl = game.VideoUrl,
              QvRating = game.QvRating,
              ReleaseDate = game.ReleaseDate,
              IsReleased = game.IsReleased,
            };

            context.Games.Add(newGame);
            foreach( var company in existingCompanies )
            {
              var companyBelongsToGame = game.GameCompanies.FirstOrDefault(c => c.IgdbCompanyId == company.IgdbCompanyId);

              if( companyBelongsToGame != null )
              {
                context.GameCompany.Add(new GameCompany
                {
                  Game = newGame,
                  Company = company
                });
              }
            }

            foreach( var genre in existingGenres )
            {
              var existingGenre = game.GameGenres.FirstOrDefault(g => g.IgdbGenreId == genre.IgdbGenreId);
              if( existingGenre != null )
              {
                context.GameGenre.Add(new GameGenre
                {
                  Game = newGame,
                  Genre = genre
                });
              }
            }

            foreach( var platform in existingPlatforms )
            {
              var existingPlatform = game.GamePlatforms.FirstOrDefault(p => p.IgdbPlatformId == platform.IgdbPlatformId);
              if( existingPlatform != null )
              {
                context.GamePlatform.Add(new GamePlatform
                {
                  Game = newGame,
                  Platform = platform
                });
              }
            }
            SaveChanges(context);
          }
        }
        SaveChanges(context);
      }

      var user = context.Users.FirstOrDefault(u=>u.Id == userId);
      if( user == null ) return;
      user.SteamID = steamId;
      SaveChanges(context);
      var library = context.GamesLibrary.Include(l=>l.GameLogs).FirstOrDefault(l => l.User == user);
      if( library == null )
      {
        library = new() { User = user, GameLogs = [], Top5Games = [] };
        context.GamesLibrary.Add(library);
        SaveChanges(context);
      }
      library.GameLogs ??= [];
      foreach( GameLog gl in finalGameLogs )
      {
        var existingGL = library.GameLogs.FirstOrDefault(g => g.IgdbId == gl.IgdbId);
        if( existingGL != null )
        {
          existingGL.Status = gl.Status;
        }
        else
        {
          var game = context.Games.FirstOrDefault(g=> g.IgdbId == gl.IgdbId);
          if( game != null )
          {
            gl.Game = game;
            library.GameLogs.Add(gl);
          }
        }
        SaveChanges(context);
      }
      watch.Stop();
      Console.Out.WriteLineAsync($"Elapsed: {watch.ElapsedMilliseconds} ms");
      return;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Console.WriteLine("start Async");
      using( var scope = serviceProvider.CreateScope() )
      {
        context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        steamAPI = scope.ServiceProvider.GetRequiredService<SteamAPI>();
        igdbService = scope.ServiceProvider.GetRequiredService<IServiceIGDB>();
        timer = new Timer(o =>
            {
              //Console.WriteLine($"queue count: {queue.Count}");
              //KeyValuePair<string, string> pair = queue.FirstOrDefault();
              //Console.WriteLine($"pair: [{pair.Key}, {pair.Value}]");
              //ImportLibraryAsync(queue);
              if( mutex )
              {
                mutex = false;
                Console.WriteLine($"queue size: {queue.Count}");
                var pair = queue.FirstOrDefault();
                if( pair.Key != null && pair.Value != null )
                {
                  ImportLibraryAsync(pair);
                  queue.Remove(pair.Key);
                }
                mutex = true;
              }
            },
            queue, TimeSpan.Zero, TimeSpan.FromSeconds(10)
          );
      }
      return Task.CompletedTask;
    }

    public void EnqueueImport(string? userId, string? steamId)
    {
      while( !mutex ) { Thread.Sleep(100); }
      mutex = false;
      Console.WriteLine($"Enqueue import, {userId}, {steamId}");
      if( userId.IsNullOrEmpty() || steamId.IsNullOrEmpty() ) return;
      queue.Add(userId, steamId);
      var first = queue.FirstOrDefault();
      Console.WriteLine($"first: [{first.Key}, {first.Value}]");
      mutex = true;
    }

    public string DequeueImport(string userId)
    {
      if( queue.TryGetValue(userId, out var steamId) )
      {
        queue.Remove(userId);
        return steamId;
      }
      return string.Empty;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      timer?.Change(Timeout.Infinite, 0);
      context = null;
      steamAPI = null;
      igdbService = null;
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      timer?.Dispose();
      GC.SuppressFinalize(this);
    }
  }
}
