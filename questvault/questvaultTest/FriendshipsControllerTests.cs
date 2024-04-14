using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;

namespace questvaultTest
{
  public class FriendshipsControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private readonly User Sender = new(){Id = "senderId"};
    private readonly User Receiver = new(){Id = "receiverId"};
    private readonly SignInManager<User> signInManager = new FakeSignInManager();
    private readonly ApplicationDbContext context;
    private readonly FriendshipsController controller;
    private static bool flagRunSettup = false;

    private void Setup()
    {
      if( !flagRunSettup )
      {
        flagRunSettup = true;
        context.Users.Add(Sender);
        context.Users.Add(Receiver);
        context.SaveChanges();
        signInManager.SignInAsync(Sender, true).Wait();
      }
    }

    public FriendshipsControllerTests(ApplicationDbContextFixture dbContextFixture)
    {
      context = dbContextFixture.DbContext;
      Setup();
      controller = new(context, signInManager);
    }

    [Fact]
    public async Task SendFriendRequestAsync_ValidArguments_CreatesFriendshipRequest()
    {
      // Arrange


      // Act
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);

      // Assert 
      var result = await context.FriendshipRequest.FirstOrDefaultAsync(f => f.SenderId == Sender.Id && f.ReceiverId == Receiver.Id);
      Assert.NotNull(result);
      Assert.Equal(Sender.Id, result.SenderId);
      Assert.Equal(Receiver.Id, result.ReceiverId);
      context.FriendshipRequest.Remove(result);
      context.SaveChanges();
    }

    [Fact]
    public async Task SendFriendRequestAsync_ValidId_ReturnsRedirectToRouteResult()
    {
      // Arrange

      // Act
      var result = await controller.SendFriendRequestAsync(Receiver.Id);

      // Assert
      Assert.IsType<RedirectToRouteResult>(result);
      var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
      Assert.Equal("Friendships", redirectToRouteResult?.RouteValues?["controller"]);
      Assert.Equal("FriendsPage", redirectToRouteResult?.RouteValues?["action"]);
      var remove = await context.FriendshipRequest.FirstOrDefaultAsync(f => f.SenderId == Sender.Id && f.ReceiverId == Receiver.Id);
      if( remove != null )
      {
        context.FriendshipRequest.Remove(remove);
        context.SaveChanges();
      }
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_ValidArguments_CreatesFriendship()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);

      // Act
      await FriendshipsController.AcceptFriendRequestAsync(Receiver.Id, Sender.Id, context);
      var result = await context.Friendship.FirstOrDefaultAsync(f => f.User1Id == Sender.Id && f.User2Id == Receiver.Id || f.User1Id == Receiver.Id && f.User2Id == Sender.Id);

      // Assert
      Assert.NotNull(result);
      Assert.True(result.User1?.Id == Sender.Id || result.User2?.Id == Sender.Id);
      Assert.True(result.User2?.Id == Receiver.Id || result.User1?.Id == Receiver.Id);
      context.Remove(result);
      context.SaveChanges();
    }

    [Fact]
    public async Task AcceptFriendRequestAsync_ValidId_ReturnsRedirectToRouteResult()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);

      // Act
      var result = await controller.AcceptFriendRequestAsync(Receiver.Id);

      // Assert
      var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
      Assert.Equal("Friendships", redirectToRouteResult?.RouteValues?["controller"]);
      Assert.Equal("FriendRequests", redirectToRouteResult?.RouteValues?["action"]);
      var remove = await context.Friendship.FirstOrDefaultAsync(f => f.User1Id == Sender.Id && f.User2Id == Receiver.Id || f.User1Id == Receiver.Id && f.User2Id == Sender.Id);
      if( remove != null )
      {
        context.Friendship.Remove(remove);
        context.SaveChanges();
      }
    }

    [Fact]
    public async Task RejectFriendRequestAsync_ValidArguments_RemovesFriendshipRequest()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);

      // Act
      await FriendshipsController.RejectFriendRequestAsync(Receiver.Id, Sender.Id, context);

      // Assert
      var result = await context.FriendshipRequest.FirstOrDefaultAsync(f=> f.SenderId == Sender.Id && f.ReceiverId == Receiver.Id);
      Assert.Null(result);
    }

    [Fact]
    public async Task RejectFriendRequestAsync_ValidId_ReturnsRedirectToRouteResult()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Receiver.Id, Sender.Id, context);

      // Act
      var result = await controller.RejectFriendRequestAsync(Receiver.Id);

      // Assert
      var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
      Assert.Equal("Friendships", redirectToRouteResult?.RouteValues?["controller"]);
      Assert.Equal("FriendRequests", redirectToRouteResult?.RouteValues?["action"]);
    }

    [Fact]
    public async Task FriendsPageAsync_UserFound_ReturnsViewWithFriends()
    {
      // Arrange
      await signInManager.SignInAsync(Sender, true);
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);
      await FriendshipsController.AcceptFriendRequestAsync(Receiver.Id, Sender.Id, context);

      // Act
      var result = await controller.FriendsPageAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<List<Friendship>>(viewResult.Model);
      Assert.Single(model);
      Assert.True(model.FirstOrDefault(m => m.User1Id == Sender.Id && m.User2Id == Receiver.Id || m.User1Id == Receiver.Id && m.User2Id == Sender.Id) != null);
      var remove = model.FirstOrDefault();
      if( remove != null )
      {
        context.Friendship.Remove(remove);
        context.SaveChanges();
      }
    }

    [Fact]
    public async Task FriendRequestsAsync_UserFound_ReturnsViewWithFriendshipRequests()
    {
      // Arrange
      await signInManager.SignInAsync(Sender, true);
      await FriendshipsController.SendFriendRequestAsync(Receiver.Id, Sender.Id, context);

      // Act
      var result = await controller.FriendRequestsAsync();

      // Assert
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsAssignableFrom<List<FriendshipRequest>>(viewResult.Model);
      Assert.Single(model);
      Assert.True(model.FirstOrDefault(m => m.SenderId == Receiver.Id && m.ReceiverId == Sender.Id) != null);
      var remove = model.FirstOrDefault();
      if( remove != null )
      {
        context.FriendshipRequest.Remove(remove);
        context.SaveChanges();
      }
    }

    [Fact]
    public async Task RemoveFriendAsync_ValidArguments_RemovesFriend()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);
      await FriendshipsController.AcceptFriendRequestAsync(Receiver.Id, Sender.Id, context);
      bool wereFriendsBefore = context.Friendship.Any(f => f.User1Id == Sender.Id && f.User2Id == Receiver.Id || f.User1Id == Receiver.Id && f.User2Id == Sender.Id);

      // Act
      await FriendshipsController.RemoveFriendAsync(Sender.Id, Receiver.Id, context);
      var result = await context.Friendship.FirstOrDefaultAsync(f => f.User1Id == Sender.Id && f.User2Id == Receiver.Id || f.User1Id == Receiver.Id && f.User2Id == Sender.Id);

      // Assert
      Assert.True(wereFriendsBefore);
      Assert.Null(result);
    }

    [Fact]
    public async Task RemoveFriendAsync_CallsCorrectRemoveFriendAsync()
    {
      // Arrange
      await FriendshipsController.SendFriendRequestAsync(Receiver.Id, Sender.Id, context);
      await FriendshipsController.AcceptFriendRequestAsync(Sender.Id, Receiver.Id, context);

      // Act
      var result = await controller.RemoveFriendAsync(Receiver.Id);

      // Assert
      Assert.NotNull(result);
      var redirectToRoute = Assert.IsType<RedirectToRouteResult>(result);
      Assert.Equal("Friendships", redirectToRoute?.RouteValues?["controller"]);
      Assert.Equal("FriendsPage", redirectToRoute?.RouteValues?["action"]);
    }
  }
}
