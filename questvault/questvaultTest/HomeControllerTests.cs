using Microsoft.AspNetCore.Mvc;
using questvault.Controllers;
using questvault.Data;

namespace questvaultTest
{
  public class HomeControllerTests : IClassFixture<ApplicationDbContextFixture>
  {
    private ApplicationDbContext _context;

    public HomeControllerTests(ApplicationDbContextFixture context)
    {
      _context = context.DbContext;
    }


    [Fact]
    public async Task Index_ReturnsViewResult()
    {
      //// Arrange
      //var controller = new HomeController(null, _context);

      //// Act
      //var result = await controller.IndexAsync();
      //var accessInstances = _context.AccessInstances;
      //// Assert
      //Assert.Equal(1, accessInstances.Count());
      //result = await controller.IndexAsync();
      //result = await controller.IndexAsync();
      //Assert.Equal(3, accessInstances.Count());
      //Assert.IsType<ViewResult>(result);
    }

    //[Fact]
    //public void Privacy_ReturnsViewResult()
    //{
    //  var controller = new HomeController(null, _context);
    //  // Act
    //  var result = controller.Privacy();
    //    // Assert
    //    Assert.IsType<ViewResult>(result);
    //}
  }
}