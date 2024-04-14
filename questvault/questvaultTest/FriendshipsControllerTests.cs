using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;
using System.Security.Claims;

namespace questvaultTest
{
  public class FriendshipsControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private readonly User Sender = new(){Id = "senderId"};
    private readonly User Receiver = new(){Id = "receiverId"};
    private readonly ClaimsPrincipal senderPrincipal;
    private readonly SignInManager<User> signInManager = new FakeSignInManager();
    private readonly ApplicationDbContext context;
    private readonly FriendshipsController controller;

    public FriendshipsControllerTests(ApplicationDbContextFixture dbContextFixture)
    {
      signInManager.SignInAsync(Sender, true);
      senderPrincipal = signInManager.ClaimsFactory.CreateAsync(Sender).Result;
      context = dbContextFixture.DbContext;
      context.Users.Add(Sender);
      context.Users.Add(Receiver);
      context.SaveChanges();
      controller = new(context, signInManager);
    }

    [Fact]
    public async Task SendFriendRequestAsync_ValidArguments_CreatesFriendshipRequest()
    {
      // Arrange
      context.Users.Add(Sender);
      context.Users.Add(Receiver);
      await context.SaveChangesAsync();

      // Act
      await FriendshipsController.SendFriendRequestAsync(Sender.Id, Receiver.Id, context);
      var result = await context.FriendshipRequest.FirstOrDefaultAsync(f => f.SenderId == Sender.Id && f.ReceiverId == Receiver.Id);

      // Assert 
      Assert.NotNull(result);
      Assert.Equal(Sender.Id, result.SenderId);
      Assert.Equal(Receiver.Id, result.ReceiverId);
    }

    [Fact]
    public async Task SendFriendRequestAsync_ValidId_ReturnsRedirectToRouteResult()
    {
      // Arrange
      await signInManager.SignInAsync(Sender, true);

      // Act
      var result = await controller.SendFriendRequestAsync(Receiver.Id);

      // Assert
      Assert.IsType<RedirectToRouteResult>(result);
      var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
      Assert.Equal("Friendships", redirectToRouteResult?.RouteValues?["controller"]);
      Assert.Equal("FriendsPage", redirectToRouteResult?.RouteValues?["action"]);
    }

  }
}
