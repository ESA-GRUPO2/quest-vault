﻿using Microsoft.AspNetCore.Identity;
using Moq;
using questvault.Data;
using questvault.Models;

namespace questvaultTest
{
  public class LibraryControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private ApplicationDbContext _context;
    private Mock<IUserConfirmation<User>> userConfirmationMock;

    public LibraryControllerTests(ApplicationDbContextFixture context)
    {
      _context = context.DbContext;
      userConfirmationMock = new Mock<IUserConfirmation<User>>(MockBehavior.Strict);
    }

    [Fact]
    public async Task UserLibrary_ReturnsZeroGames_WhenUserHasNoGamesInLibrary()
    {
      //var context = new Mock<HttpContext>();
      //var contextAccessor = new Mock<IHttpContextAccessor>();
      //contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      //var _fakeSignInManager = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      //// Arrange
      //var controller = new LibraryController(_context, _fakeSignInManager);

      //// Act
      //var actionResult = await controller.UserLibrary("UNITTESTER1", 1, "");

      //// Assert
      //var viewResult = Assert.IsType<ViewResult>(actionResult);
      //var model = viewResult.Model as GameViewData;
      //Assert.NotNull(model);
      //Assert.Equal(0, model.NumberOfResults);
    }

  }
}
