using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;
using questvault.Services;



namespace questvaultTest
{
  public class BackofficeControllerTests(ApplicationDbContextFixture dbContextFixture) : IClassFixture<ApplicationDbContextFixture>
  {
    private readonly BackofficeController controller = new(dbContextFixture.DbContext, new Mock<IServiceIGDB>().Object, new FakeSignInManager());

    [Fact]
    public async Task MakeModerator_UserFound_ClearanceUpdatedTo1()
    {
      // Arrange
      // Create a user with the specific id
      var user = new User { Id = "Moderator", Clearance = 0 };
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MakeModerator_UserFound_ClearanceUpdatedTo1")
            .Options;

      using( var context = new ApplicationDbContext(options) )
      {
        context.Users.Add(user);
        context.SaveChanges();
      }

      using( var context = new ApplicationDbContext(options) )
      {
        // Act
        await BackofficeController.GiveModerator("Moderator", context);

        int result = context.Users.First(u => u.Id == "Moderator").Clearance;

        Assert.Equal(1, result);
      }
    }

    [Fact]
    public async Task MakeModerator_ValidId_ReturnsRedirectToActionResult()
    {
      // Arrange
      var id = "validIdModerator";
      var user = new User { Id = id };
      dbContextFixture.DbContext.Users.Add(user);
      dbContextFixture.DbContext.SaveChanges();

      // Act
      var result = await controller.GiveModerator(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      Assert.Equal("Profile", ( (RedirectToActionResult) result ).ActionName);
      Assert.Equal("User", ( (RedirectToActionResult) result ).ControllerName);
    }

    [Fact]
    public async Task MakeModeratorAll_ReturnsRedirectToAction()
    {
      // Arrange
      var id = "someId";

      // Act
      var result = await controller.GiveModeratorAll(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("AllUsers", redirectToActionResult.ActionName);
      Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task MakeAdmin_UserFound_ClearanceUpdatedTo1()
    {
      // Arrange
      // Create a user with the specific id
      var user = new User { Id = "Admin", Clearance = 0 };
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MakeAdmin_UserFound_ClearanceUpdatedTo1")
            .Options;

      using( var context = new ApplicationDbContext(options) )
      {
        context.Users.Add(user);
        context.SaveChanges();
      }

      using( var context = new ApplicationDbContext(options) )
      {
        // Act
        await BackofficeController.GiveAdmin("Admin", context);

        int result = context.Users.First(u => u.Id == "Admin").Clearance;

        Assert.Equal(2, result);
      }
    }

    [Fact]
    public async Task MakeAdmin_ValidId_ReturnsRedirectToActionResult()
    {
      // Arrange
      var id = "validIdAdmin";
      var user = new User { Id = id };
      dbContextFixture.DbContext.Users.Add(user);
      dbContextFixture.DbContext.SaveChanges();

      // Act
      var result = await controller.GiveAdmin(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      Assert.Equal("Profile", ( (RedirectToActionResult) result ).ActionName);
      Assert.Equal("User", ( (RedirectToActionResult) result ).ControllerName);
    }

    [Fact]
    public async Task MakeAdminAll_ReturnsRedirectToAction()
    {
      // Arrange
      var id = "someId";

      // Act
      var result = await controller.GiveAdminAll(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("AllUsers", redirectToActionResult.ActionName);
      Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task RemovePermissions_UserFound_ClearanceUpdatedTo0()
    {
      // Arrange
      var id = "RemovePermissions";
      var user = new User { Id = id, Clearance = 1 };
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RemovePermissions_UserFound_ClearanceUpdatedTo0")
            .Options;

      using( var context = new ApplicationDbContext(options) )
      {
        context.Users.Add(user);
        context.SaveChanges();
      }

      using( var context = new ApplicationDbContext(options) )
      {
        // Act
        await BackofficeController.RemovePermissions(id, context);

        int result = context.Users.First(u => u.Id == id).Clearance;

        // Assert
        Assert.Equal(0, result);
      }
    }

    [Fact]
    public async Task RemovePermissionsAll_RedirectsToAllUsers()
    {
      // Arrange
      var id = "testId";

      // Act
      var result = await controller.RemovePermissionsAll(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("AllUsers", redirectToActionResult.ActionName);
      Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task RemovePermissions_RedirectsToProfile()
    {
      // Arrange
      var id = "testId";

      // Act
      var result = await controller.RemovePermissions(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("Profile", redirectToActionResult.ActionName);
      Assert.Equal("User", redirectToActionResult.ControllerName);
      Assert.Equal(id, redirectToActionResult?.RouteValues?["id"]);
    }

    [Fact]
    public async Task LockoutUser_WithValidId_LocksUser()
    {
      // Arrange
      var id = "Lockedout";
      var user = new User { Id = id, LockoutEnabled = false };
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "LockoutUser_WithValidId_LocksUser")
            .Options;

      using( var context = new ApplicationDbContext(options) )
      {
        context.Users.Add(user);
        context.SaveChanges();
      }

      using( var context = new ApplicationDbContext(options) )
      {
        // Act
        await BackofficeController.LockoutUser(id, context);

        bool result = context.Users.First(u => u.Id == id).LockoutEnabled;

        // Assert
        Assert.True(result);
      }
    }

    [Fact]
    public async Task RemoveLockoutUser_WithValidId_RemovesLockout()
    {
      // Arrange
      var id = "Lockedout";
      var user = new User { Id = id, LockoutEnabled = true };
      var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "RemoveLockoutUser_WithValidId_RemovesLockout")
            .Options;

      using( var context = new ApplicationDbContext(options) )
      {
        context.Users.Add(user);
        context.SaveChanges();
      }

      using( var context = new ApplicationDbContext(options) )
      {
        // Act
        await BackofficeController.LockoutUser(id, context);

        bool result1 = context.Users.First(u => u.Id == id).LockoutEnabled;

        await BackofficeController.RemoveLockoutUser(id, context);

        bool result2 = context.Users.First(u=> u.Id == id).LockoutEnabled;

        // Assert
        Assert.True(result1);
        Assert.True(!result2);
      }
    }

    [Fact]
    public async Task LockoutUserAll_RedirectsToAllUsers()
    {
      // Arrange
      var id = "someId";

      // Act
      var result = await controller.LockoutUserAll(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("AllUsers", redirectToActionResult.ActionName);
      Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task LockoutUser_RedirectsToProfile()
    {
      // Arrange
      var id = "someId";

      // Act
      var result = await controller.LockoutUser(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("Profile", redirectToActionResult.ActionName);
      Assert.Equal("User", redirectToActionResult.ControllerName);
      Assert.Equal("someId", redirectToActionResult.RouteValues["id"]);
    }

    [Fact]
    public async Task RemoveLockoutUserAll_RedirectsToAllUsers()
    {
      // Arrange
      var id = "someid";

      // Act
      var result = await controller.RemoveLockoutUserAll(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("AllUsers", redirectToActionResult.ActionName);
      Assert.Equal("Home", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task RemoveLockoutUser_RedirectsToProfile()
    {
      // Arrange
      var id = "someId";

      // Act
      var result = await controller.RemoveLockoutUser(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("Profile", redirectToActionResult.ActionName);
      Assert.Equal("User", redirectToActionResult.ControllerName);
      Assert.Equal("someId", redirectToActionResult.RouteValues["id"]);
    }

    [Fact]
    public async Task RemoveReview_WithNonExistingGameLogId_ReturnsNotFound()
    {
      // Arrange
      var id = 1; // Assuming 1 is a non-existing GameLogId

      // Act
      var result = await controller.RemoveReview(id);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task RemoveReview_RedirectsToDetails()
    {
      // Arrange
      var id = 1;
      var userId = "123";
      var game = new Game { GameId = 123, IgdbId = 123, IgdbRating = 0, IsReleased = true};
      dbContextFixture.DbContext.Games.Add(game);
      var gameLog = new GameLog { GameLogId = id, IgdbId = 123 , Game = game };
      var user = new User{ Id = userId };
      var library = new GamesLibrary { User = user, GameLogs = [gameLog] };
      dbContextFixture.DbContext.Users.Add(user);
      dbContextFixture.DbContext.GamesLibrary.Add(library);
      await dbContextFixture.DbContext.SaveChangesAsync();

      // not sure if its fair of me to do this here but i have no other way of making sure if its in the db
      var res = await dbContextFixture.DbContext.GameLog.FirstOrDefaultAsync(gl => gl.GameLogId == id);
      Assert.NotNull(res);

      // Act
      var result = await controller.RemoveReview(id);

      // Assert
      Assert.IsType<RedirectToActionResult>(result);
      var redirectToActionResult = (RedirectToActionResult)result;
      Assert.Equal("Details", redirectToActionResult.ActionName);
      Assert.Equal("Games", redirectToActionResult.ControllerName);
      Assert.Equal((long) 123, redirectToActionResult?.RouteValues?["id"]);
    }
  }
}
