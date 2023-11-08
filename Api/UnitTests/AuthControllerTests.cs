using Api.Controllers;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task LoginAsync_WhenValidSchemaAndCredentials_ShouldReturnOkWithLoginDto()
        {
            //Arrange
            UserEntity userEntity = new UserEntity() 
            { 
                Username = "username",
                Password = "Password123@",
                Score = 0
            };
            LoginSchema loginSchema = new LoginSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<LoginSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);
            iUserServiceMock.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            AuthController authController = new AuthController(iUserServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await authController.LoginAsync(loginSchema);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<LoginDto>(okObjectResult.Value);
            LoginDto loginDto = okObjectResult.Value as LoginDto;

            Assert.Equal(loginDto.Username, loginSchema.Username);
            Assert.Equal(loginDto.Score, userEntity.Score);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidModel_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            AuthController authController = new AuthController(It.IsAny<IUserService>(), It.IsAny<IJwtService>());
            authController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await authController.LoginAsync(It.IsAny<LoginSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(1, statusDto.Code);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidSchema_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<LoginSchema>())).Returns(false);

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.LoginAsync(It.IsAny<LoginSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(6, statusDto.Code);
        }
        [Fact]
        public async Task LoginAsync_WhenUserIsNotFound_ShouldReturnUnauthorizedWithStatusDto()
        {
            //Arrange
            LoginSchema loginSchema = new LoginSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<LoginSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.LoginAsync(loginSchema);

            //Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            UnauthorizedObjectResult unauthorizedObjectResult = result as UnauthorizedObjectResult;

            Assert.IsType<StatusDto>(unauthorizedObjectResult.Value);
            StatusDto statusDto = unauthorizedObjectResult.Value as StatusDto;

            Assert.Equal(8, statusDto.Code);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidPassword_ShouldReturnUnauthorizedWithStatusDto()
        {
            //Arrange
            LoginSchema loginSchema = new LoginSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "username",
                Password = "Password123@",
                Score = 0
            };
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<LoginSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);
            iUserServiceMock.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.LoginAsync(loginSchema);

            //Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            UnauthorizedObjectResult unauthorizedObjectResult = result as UnauthorizedObjectResult;

            Assert.IsType<StatusDto>(unauthorizedObjectResult.Value);
            StatusDto statusDto = unauthorizedObjectResult.Value as StatusDto;

            Assert.Equal(8, statusDto.Code);
        }
        [Fact]
        public async Task RegisterAsync_WhenValidSchemaAndUsernameNotTaken_ShouldReturnCreatedWithRegisterDto()
        {
            //Arrange
            RegisterSchema registerSchema = new RegisterSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<RegisterSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);
            iUserServiceMock.Setup(x => x.HashPasswordAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(It.IsAny<string>());
            iUserServiceMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            AuthController authController = new AuthController(iUserServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await authController.RegisterAsync(registerSchema);

            //Assert
            Assert.IsType<CreatedResult>(result);
            CreatedResult createdResult = result as CreatedResult;

            Assert.IsType<RegisterDto>(createdResult.Value);
            RegisterDto registerDto = createdResult.Value as RegisterDto;

            Assert.Equal(registerSchema.Username, registerDto.Username);
        }
        [Fact]
        public async Task RegisterAsync_WhenInvalidModel_ShouldReturnBadWithStatusDto()
        {
            AuthController authController = new AuthController(It.IsAny<IUserService>(), It.IsAny<IJwtService>());
            authController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await authController.RegisterAsync(It.IsAny<RegisterSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(1, statusDto.Code);
        }
        [Fact]
        public async Task RegisterAsync_WhenInvalidSchema_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<LoginSchema>())).Returns(false);

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.RegisterAsync(It.IsAny<RegisterSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(6, statusDto.Code);
        }
        [Fact]
        public async Task RegisterAsync_WhenUsernameIsTaken_ShouldReturnConflictWithStatusDto()
        {
            //Arrange
            RegisterSchema registerSchema = new RegisterSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<RegisterSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new UserEntity());

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.RegisterAsync(registerSchema);

            //Assert
            Assert.IsType<ConflictObjectResult>(result);
            ConflictObjectResult conflictObjectResult = result as ConflictObjectResult;

            Assert.IsType<StatusDto>(conflictObjectResult.Value);
            StatusDto statusDto = conflictObjectResult.Value as StatusDto;

            Assert.Equal(9, statusDto.Code);
        }
        [Fact]
        public async Task RegisterAsync_WhenServiceUnableToCreateUser_ShouldThrowException()
        {
            //Arrange
            RegisterSchema registerSchema = new RegisterSchema()
            {
                Username = "username",
                Password = "Password123@"
            };
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.Validate(It.IsAny<RegisterSchema>())).Returns(true);
            iUserServiceMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);
            iUserServiceMock.Setup(x => x.HashPasswordAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(It.IsAny<string>());
            iUserServiceMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(false);

            AuthController authController = new AuthController(iUserServiceMock.Object, It.IsAny<IJwtService>());

            //Act
            await Assert.ThrowsAsync<Exception>(() => authController.RegisterAsync(registerSchema));
        }
        [Fact]
        public async Task ValidateTokenAsync_WhenSuccess_ShouldReturnNoContent()
        {
            //Arrange
            AuthController authController = new AuthController(It.IsAny<IUserService>(), It.IsAny<IJwtService>());

            //Act
            IActionResult result = await authController.ValidateTokenAsync();

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
