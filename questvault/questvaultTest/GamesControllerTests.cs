using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using questvault.Controllers;
using questvault.Data;
using questvault.Models;
using questvault.Services;

namespace questvaultTest
{
  public class GamesControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private ApplicationDbContext _context;
    private Mock<IUserConfirmation<User>> userConfirmationMock;
    public GamesControllerTests(ApplicationDbContextFixture context)
    {
      _context = context.DbContext;
      //var userManagerMock = new Mock<UserManager<User>>(MockBehavior.Strict, null, null, null, null, null, null, null, null);
      userConfirmationMock = new Mock<IUserConfirmation<User>>(MockBehavior.Strict);


    }

    [Fact]
    public async Task Index_ReturnsZeroGames()
    {
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





      // Chama o método Index() e aguarda a conclusão da tarefa
      //var result =  controller.Index("released", "", "");

      // Obtém o resultado da tarefa
      // Chama o método Index() e aguarda a conclusão da tarefa
      var actionResult = await controller.Index("unreleased", "", "");

      // Verifica se o resultado é do tipo ViewResult
      var viewResult = Assert.IsType<ViewResult>(actionResult);

      // Verifica se o modelo é do tipo GameViewData
      var model = viewResult.Model as GameViewData;
      Assert.NotNull(model); // Verifica se o modelo não é nulo

      // Agora você pode fazer outras asserções sobre o modelo
      Assert.Equal(0, model.NumberOfResults);

    }

    [Fact]
    public async Task Index_ReturnsThreeGames()
    {
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





      // Chama o método Index() e aguarda a conclusão da tarefa
      //var result =  controller.Index("released", "", "");

      // Obtém o resultado da tarefa
      // Chama o método Index() e aguarda a conclusão da tarefa
      var actionResult = await controller.Index("released", "", "Linux");

      // Verifica se o resultado é do tipo ViewResult
      var viewResult = Assert.IsType<ViewResult>(actionResult);

      // Verifica se o modelo é do tipo GameViewData
      var model = viewResult.Model as GameViewData;
      // Assert.NotNull(model); // Verifica se o modelo não é nulo

            // Agora você pode fazer outras asserções sobre o modelo
            Assert.Equal(3, model.NumberOfResults);

    }

    [Fact]
    public async Task Results_ReturnsBioshockGame()
    {
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





      // Chama o método Index() e aguarda a conclusão da tarefa
      //var result =  controller.Index("released", "", "");

      // Obtém o resultado da tarefa
      // Chama o método Index() e aguarda a conclusão da tarefa
      var actionResult = await controller.Results("BioShock");

      // Verifica se o resultado é do tipo ViewResult
      var viewResult = Assert.IsType<ViewResult>(actionResult);

      // Verifica se o modelo é do tipo GameViewData
      var model = viewResult.Model as GameViewData;
      //Assert.NotNull(model); // Verifica se o modelo não é nulo

      // Agora você pode fazer outras asserções sobre o modelo
      //Assert.Equal(1, model.NumberOfResults);

    }
    [Fact]
    public async Task Results_ReturnsGame1()
    {
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





      // Chama o método Index() e aguarda a conclusão da tarefa
      //var result =  controller.Index("released", "", "");

      // Obtém o resultado da tarefa
      // Chama o método Index() e aguarda a conclusão da tarefa
      var actionResult = await controller.Results("Game 1");

      // Verifica se o resultado é do tipo ViewResult
      var viewResult = Assert.IsType<ViewResult>(actionResult);

      // Verifica se o modelo é do tipo GameViewData
      var model = viewResult.Model as GameViewData;
      Assert.NotNull(model); // Verifica se o modelo não é nulo

      // Agora você pode fazer outras asserções sobre o modelo
      Assert.Equal(1, model.NumberOfResults);

    }
    [Fact]
    public async Task Results_ReturnsAllGames()
    {
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





      // Chama o método Index() e aguarda a conclusão da tarefa
      //var result =  controller.Index("released", "", "");

      // Obtém o resultado da tarefa
      // Chama o método Index() e aguarda a conclusão da tarefa
      var actionResult = await controller.Results("");

      // Verifica se o resultado é do tipo ViewResult
      var viewResult = Assert.IsType<ViewResult>(actionResult);

      // Verifica se o modelo é do tipo GameViewData
      var model = viewResult.Model as GameViewData;
      //Assert.NotNull(model); // Verifica se o modelo não é nulo

      // Agora você pode fazer outras asserções sobre o modelo
      // Assert.Equal(10, model.NumberOfResults);

    }
    //[Fact]
    //public async Task ResultsPost_AddsThreeGames()
    //{
    //    var context = new Mock<HttpContext>();
    //    var contextAccessor = new Mock<IHttpContextAccessor>();
    //    contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
    //    var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

    //    var mockIGDBService = new Mock<IServiceIGDB>();

    //    var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);





    //    // Chama o método Index() e aguarda a conclusão da tarefa
    //    //var result =  controller.Index("released", "", "");

    //    // Obtém o resultado da tarefa
    //    // Chama o método Index() e aguarda a conclusão da tarefa
    //    var actionResult = await controller.ResultsPost("Sekiro: Shadows Die Twice");

    //    // Verifica se o resultado é do tipo ViewResult
    //    var viewResult = Assert.IsType<ViewResult>(actionResult);

    //    // Verifica se o modelo é do tipo GameViewData
    //    var model = viewResult.Model as GameViewData;
    //    Assert.NotNull(model); // Verifica se o modelo não é nulo

    //    // Agora você pode fazer outras asserções sobre o modelo
    //    Assert.Equal(3, model.NumberOfResults);

    //}

    [Fact]
    public void Search_ReturnsNotFound_WhenSearchTermIsNull()
    {
      // Arrange
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);

      string searchTerm = null;

      // Act
      var result = controller.Search(searchTerm);

      // Assert
      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Search_ReturnsJsonResult_WithMatchingGames()
    {

      // Arrange
      var context = new Mock<HttpContext>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);

      var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

      var mockIGDBService = new Mock<IServiceIGDB>();

      var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);
      string searchTerm = "Grand";

      // Act
      var result = controller.Search(searchTerm);

      // Assert
      var jsonResult = Assert.IsType<JsonResult>(result);
      var model = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);
      //Assert.Equal(2, model.Count());
    }
  }
}
