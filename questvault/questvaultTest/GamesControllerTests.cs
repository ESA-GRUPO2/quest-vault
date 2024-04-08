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
        public async Task Index_ReturnsOneUnreleasedGame()
        {
            var context = new Mock<HttpContext>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
            var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

            var mockIGDBService = new Mock<IServiceIGDB>();

            var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);






            var actionResult = await controller.Index(1, "unreleased", "", "");


            var viewResult = Assert.IsType<ViewResult>(actionResult);


            var model = viewResult.Model as GameViewData;
            Assert.NotNull(model);


            Assert.Equal(1, model.NumberOfResults);

        }

        [Fact]
        public async Task Index_ReturnsOneReleasedGame()
        {
            var context = new Mock<HttpContext>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
            var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

            var mockIGDBService = new Mock<IServiceIGDB>();

            var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);






            var actionResult = await controller.Index(1, "released", "", "");


            var viewResult = Assert.IsType<ViewResult>(actionResult);


            var model = viewResult.Model as GameViewData;
            Assert.NotNull(model); // Verifica se o modelo não é nulo
            Assert.Equal(1, model.NumberOfResults);

        }

        [Fact]
        public async Task Results_ReturnsWithNameGame1()
        {
            var context = new Mock<HttpContext>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);
            var faketaxi = new TestSignInManager(contextAccessor.Object, userConfirmationMock.Object);

            var mockIGDBService = new Mock<IServiceIGDB>();

            var controller = new GamesController(_context, mockIGDBService.Object, faketaxi);






            var actionResult = await controller.Results("Game 1", 1);


            var viewResult = Assert.IsType<ViewResult>(actionResult);

            var model = viewResult.Model as GameViewData;

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





            var actionResult = await controller.Results("", 1);

            var viewResult = Assert.IsType<ViewResult>(actionResult);

            var model = viewResult.Model as GameViewData;

            Assert.Equal(2, model.NumberOfResults);

        }


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
            string searchTerm = "Game";

            // Act
            var result = controller.Search(searchTerm);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<object>>(jsonResult.Value);
            Assert.Equal(2, model.Count());
        }
    }
}
