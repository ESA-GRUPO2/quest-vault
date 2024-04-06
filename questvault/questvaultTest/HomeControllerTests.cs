using Microsoft.AspNetCore.Mvc;
using questvault.Controllers;
using questvault.Data;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using questvault.Models;

namespace questvaultTest
{
    public class HomeControllerTests : IClassFixture<ApplicationDbContextFixture>
    {
        private ApplicationDbContext _context;

        public HomeControllerTests(ApplicationDbContextFixture context)
        {
            _context = context.DbContext;
        }


        // Nível 1
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);

            var result = controller.IndexAsync();

            var viewResult = Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}