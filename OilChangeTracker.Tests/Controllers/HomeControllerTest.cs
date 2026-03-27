using Microsoft.VisualStudio.TestTools.UnitTesting;
using OilChangeTracker.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace OilChangeTracker.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();
            
            // Act
            IActionResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            IActionResult actionResult = controller.About();
            var result = actionResult as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData);
            Assert.AreEqual("Tracking oil changes since 2019!", result.ViewData["Message"]);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            IActionResult result = controller.Contact();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
