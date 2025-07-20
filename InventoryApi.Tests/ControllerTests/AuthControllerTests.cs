using Xunit;
using InventoryApi.Controllers;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public void Register_ReturnsOkResult_WhenUserIsValid()
        {
            // Arrange
            var controller = new AuthController(); // This works only if AuthController has no dependencies
            var user = new User
            {
                Username = "testuser",
                Password = "testpass"
            };

            // Act
            var result = controller.Register(user);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
