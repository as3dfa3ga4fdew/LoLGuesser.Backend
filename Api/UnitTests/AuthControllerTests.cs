using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace UnitTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task LoginAsync_WhenValidCredentialsAndSchema_ShouldReturnIActionResultOk()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "", Password = "" };
            Mock<IAuthService> iAuthServiceMock = new Mock<IAuthService>();
            iAuthServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginSchema>())).ReturnsAsync(new OkObjectResult(null));

            AuthController authController = new AuthController(iAuthServiceMock.Object);

            //Act
            IActionResult result = await authController.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidSchema_ShouldReturnIActionResultBad()
        {
            //Arrange
            LoginSchema schema = null;
            Mock<IAuthService> iAuthServiceMock = new Mock<IAuthService>();
            iAuthServiceMock.Setup(x => x.LoginAsync(It.IsAny<LoginSchema>())).ReturnsAsync(new OkObjectResult(null));

            AuthController authController = new AuthController(iAuthServiceMock.Object);
            authController.ModelState.AddModelError("", "");
            //Act
            IActionResult result = await authController.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_WhenValidCredentialsAndSchema_ShouldReturnIActionResultOk()
        {
            //Arrange
            RegisterSchema schema = new RegisterSchema() { Username = "", Password = "" };
            Mock<IAuthService> iAuthServiceMock = new Mock<IAuthService>();
            iAuthServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterSchema>())).ReturnsAsync(new OkObjectResult(null));

            AuthController authController = new AuthController(iAuthServiceMock.Object);

            //Act
            IActionResult result = await authController.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_WhenInvalidSchema_ShouldReturnIActionResultBad()
        {
            //Arrange
            RegisterSchema schema = null;
            Mock<IAuthService> iAuthServiceMock = new Mock<IAuthService>();
            iAuthServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegisterSchema>())).ReturnsAsync(new OkObjectResult(null));

            AuthController authController = new AuthController(iAuthServiceMock.Object);
            authController.ModelState.AddModelError("", "");
            //Act
            IActionResult result = await authController.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
