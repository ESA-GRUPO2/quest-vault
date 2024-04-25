using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;

namespace questvaultTest
{
  public class StatisticsControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private readonly SignInManager<User> signInManager = new FakeSignInManager();
    private readonly ApplicationDbContext context;
    private readonly StatisticsController controller;
    private readonly User user = new() { UserName = "admin" , Clearance = 2 };

    public StatisticsControllerTests(ApplicationDbContextFixture dbContextFixture)
    {
      context = dbContextFixture.DbContext;
      //context.Users.Add(user);
      controller = new(context, signInManager);
    }

    // 1. Test that the method correctly handles the case when the user is null and redirects to the login page.
    [Fact]
    public async Task StatisticsPageAsync_UserIsNull_RedirectsToLoginPage()
    {
      // Arrange
      User user = null;

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var redirectResult = Assert.IsType<RedirectResult>(result);
      Assert.Equal("/Identity/Account/Login", redirectResult.Url);
    }

    // 2. Test that the method correctly handles the case when the user's clearance is less than 1 and redirects to the login page.
    [Fact]
    public async Task StatisticsPageAsync_UserIsNotAdmin_RedirectsToLoginPage()
    {
      // Arrange
      //Assert.True(context.Users.Any(u => u.Id.Equals(user.Id)));
      user.Clearance = 0;
      await signInManager.SignInAsync(user, false);

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var redirectResult = Assert.IsType<RedirectResult>(result);
      Assert.Equal("/Identity/Account/Login", redirectResult.Url);
      user.Clearance = 2;
    }

    // 8. Test that the method creates the Statistics object with the expected data.
    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsObject()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      Assert.NotNull(model);
    }

    // 3. Test that the method fetches and processes registered accounts from the context correctly.
    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsRegisteredUsersCount()
    {
      // Before we arrange, we know that there are 2 users in the database.
      // These are inserted automatically to test Identity, so we need to clear them so we can get the proper results.
      var prevUsers = context.Users;
      context.Users.RemoveRange(prevUsers);
      context.SaveChanges();

      // Arrange
      var user = new User { Id = "Admin", Clearance = 2 };
      await signInManager.SignInAsync(user, false);
      var registeredAccounts = new List<User>
      {
        new() { Id = "validId", Clearance = 0 },
        user,
        new() { Id = "validId2", Clearance = 0 }
      };
      context.Users.AddRange(registeredAccounts);
      context.SaveChanges();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      Assert.NotNull(model);
      Assert.Equal(3, model.registeredUsersCount);
      context.Users.RemoveRange(registeredAccounts);
    }

    // 4. Test that the method retrieves and processes game log ratings from the context properly.
    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsGameRatingAverage()
    {
      // Arrange
      var game1 = new Game{ GameId = 123, IgdbId = 123, IgdbRating = 0, IsReleased = true };
      var game2 = new Game{ GameId = 124, IgdbId = 124, IgdbRating = 0, IsReleased = true };
      var gl1 = new GameLog{ IgdbId = 123, Game = game1, User = user, Rating = 5};
      var gl2 = new GameLog{ IgdbId = 124, Game = game2, User = user, Rating = 1};
      await signInManager.SignInAsync(user, false);
      context.Games.AddRange([game1, game2]);
      context.GameLog.AddRange([gl1, gl2]);
      context.SaveChanges();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      Assert.Equal(3, model.gameRatingAverage);
    }


    // 5. Test that the method fetches login instances and access instances from the context accurately.
    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsLoginDateList()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);
      var date = DateOnly.FromDateTime(DateTime.Now);
      context.Users.Add(user);
      var logginInstance = new LoginInstance() { UserId = user.Id, LoginDate = date };
      context.LogginInstances.Add(logginInstance);
      await context.SaveChangesAsync();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      var login = Assert.Single(model.LoginDateList);
      Assert.Equal(date.ToString(), login);
      context.Users.Remove(user);
      context.LogginInstances.Remove(logginInstance);
      context.SaveChanges();
    }

    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsLoginDateCountList()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);
      context.Users.Add(user);
      var date = DateOnly.FromDateTime(DateTime.Now);
      var loginInstance = new LoginInstance() { UserId = user.Id, LoginDate = date };
      context.LogginInstances.Add(loginInstance);
      await context.SaveChangesAsync();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      var login = Assert.Single(model.LoginDateCountList);
      Assert.Equal(1, login);
      context.Users.Remove(user);
      context.LogginInstances.Remove(loginInstance);
      context.SaveChanges();
    }

    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsAccessDateCountList()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);
      var date = DateOnly.FromDateTime(DateTime.Now);
      var accessInstance = new AccessInstance() { AccessDate = date };
      context.AccessInstances.Add(accessInstance);
      context.SaveChanges();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      var access = Assert.Single(model.AccessDateCountList);
      Assert.Equal(1, access);
      context.AccessInstances.Remove(accessInstance);
      context.SaveChanges();
    }

    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsAccessDateList()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);
      var date = DateOnly.FromDateTime(DateTime.Now);
      var accessInstance = new AccessInstance() { AccessDate = date };
      context.AccessInstances.Add(accessInstance);
      context.SaveChanges();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      var access = Assert.Single(model.AccessDateList);
      Assert.Equal(date.ToString(), access);
      context.AccessInstances.Remove(accessInstance);
      context.SaveChanges();
    }

    [Fact]
    public async Task StatisticsPageAsync_ReturnsStatisticsGenreNamesAndCount()
    {
      // Arrange
      await signInManager.SignInAsync(user, false);
      Genre action = new(){ GenreName = "Action" };
      Genre rpg = new(){ GenreName = "RPG" };
      Game game1 = new() { Name = "Game1", IgdbId = 1 };
      Game game2 = new(){ Name = "Game2", IgdbId = 2};
      GameGenre gg1 = new(){ Game = game1, Genre = action };
      GameGenre gg2 = new(){ Game = game2, Genre = rpg };
      GameGenre gg3 = new(){ Game = game1, Genre = rpg };
      GameLog gl1 = new () { Game = game1, User = user };
      GameLog gl2 = new () { Game = game2, User = user };

      context.Genres.AddRange([action, rpg]);
      context.Games.AddRange([game1, game2]);
      context.GameGenre.AddRange([gg1, gg2, gg3]);
      context.Users.Add(user);
      context.GameLog.AddRange([gl1, gl2]);
      context.SaveChanges();

      // Act
      var result = await controller.StatisticsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<Statistics>(viewResult.Model);
      Assert.Equal(2, model.GenreNames.Count);
      Assert.Equal(rpg.GenreName, model.GenreNames[0]);
      Assert.Equal(action.GenreName, model.GenreNames[1]);

      Assert.Equal(2, model.GenreCount.Count);
      Assert.Equal(2, model.GenreCount[0]);
      Assert.Equal(1, model.GenreCount[1]);

      // Cleanup
      context.Genres.RemoveRange([action, rpg]);
      context.Games.RemoveRange([game1, game2]);
      context.GameGenre.RemoveRange([gg1, gg2, gg3]);
      context.Users.Remove(user);
      context.GameLog.RemoveRange([gl1, gl2]);
      context.SaveChanges();
    }
    // 6. Test that the method calculates the top 5 genres with their counts accurately.
    // 7. Test that the method processes login dates and access dates correctly.
  }
}
