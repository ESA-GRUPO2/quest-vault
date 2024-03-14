using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using questvault.Controllers;
using questvault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questvaultTest
{
    public class HomeControllerTest
    {
        public class HomeControllerTests
        {
            [Fact]
            public void Index_ReturnsViewResult()
            {
                // Arrange
               
                var controller = new HomeController(null);

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsType<ViewResult>(result);
            }

            [Fact]
            public void Privacy_ReturnsViewResult()
            {
                // Arrange
                var controller = new HomeController(null);

                // Act
                var result = controller.Privacy();

                // Assert
                Assert.IsType<ViewResult>(result);
            }

            [Fact]
            public void GameResults_ReturnsViewResult()
            {
                // Arrange
                var controller = new HomeController(null);

                // Act
                var result = controller.GameResults();

                // Assert
                Assert.IsType<ViewResult>(result);
            }

            [Fact]
            public void GameDetails_ReturnsViewResult()
            {
                // Arrange
                var controller = new HomeController(null);

                // Act
                var result = controller.GameDetails();

                // Assert
                Assert.IsType<ViewResult>(result);
            }

            [Fact]
            public void Dashboard_ReturnsViewResult()
            {
                // Arrange
                var controller = new HomeController(null);

                // Act
                var result = controller.Dashboard();

                // Assert
                Assert.IsType<ViewResult>(result);
            }

        }

    }
}
