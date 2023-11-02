using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
    public class AuthServiceTests
    {

        [Fact]
        public async Task LoginAsync_WhenValidSchemaAndCredentials_ShouldReturnIActionResultOkWithLoginDto()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "User123", Password = "Password123@"};
            UserEntity userEntity = new UserEntity() { Id = Guid.NewGuid(), Username = "user12", Password = "$2a$04$MeCyGIa1uoi7s1ooG6cpwuDTyy8tbGrI/SEUTDA9qVAdxDDEO68CK", Score = 0 };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsAssignableFrom<LoginDto>(okObjectResult.Value);

            LoginDto loginDtoResult = okObjectResult.Value as LoginDto;
            Assert.NotNull(loginDtoResult);
            Assert.Equal(userEntity.Username, loginDtoResult.Username);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidUsername_ShouldReturnIActionResultBad()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "User", Password = "Password123@" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task LoginAsync_WhenInvalidPassword_ShouldReturnIActionResultBad()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "User123", Password = "Password" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task LoginAsync_WhenUserDoesNotExist_ShouldReturnIActionResultUnauthorized()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "user121", Password = "Password123@" };
            
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<UserEntity>());

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }
        [Fact]
        public async Task LoginAsync_WhenPasswordDoesNotMatch_ShouldReturnIActionResultUnauthorized()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "user12", Password = "Password123@1" };
            UserEntity userEntity = new UserEntity() { Id = Guid.NewGuid(), Username = "user12", Password = "$2a$04$MeCyGIa1uoi7s1ooG6cpwuDTyy8tbGrI/SEUTDA9qVAdxDDEO68CK", Score = 0 };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.LoginAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }
        [Fact]
        public async Task LoginAsync_WhenSchemaIsNull_ShouldThrowArgumentNullException()
        {
            //Arange
            LoginSchema schema = null;
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
          
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => authService.LoginAsync(schema));
        }
        [Fact]
        public async Task RegisterAsync_WhenValidSchemaAndUsernameIsNotTaken_ShouldReturnIActionResultOkWithRegisterDto()
        {
            //Arrange
            RegisterSchema schema = new RegisterSchema() { Username = "user122", Password = "Password123@" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            iJwtServiceMock.Setup(x => x.Create(It.IsAny<List<Claim>>())).Returns(It.IsAny<string>());

            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);
            iUserRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsAssignableFrom<RegisterDto>(okObjectResult.Value);

            RegisterDto registerDtoResult = okObjectResult.Value as RegisterDto;
            Assert.NotNull(registerDtoResult);
            Assert.Equal(schema.Username, registerDtoResult.Username);
        }
        [Fact]
        public async Task RegisterAsync_WhenInvalidUsername_ShouldReturnIActionResultBad()
        {
            //Arrange
            RegisterSchema schema = new RegisterSchema() { Username = "user1", Password = "Password123@" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_WhenInvalidPassword_ShouldReturnIActionResultBad()
        {
            //Arrange
            RegisterSchema schema = new RegisterSchema() { Username = "user11", Password = "Password123" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_WhenUsernameAlreadyExists_ShouldReturnIActionResultConflict()
        {
            //Arrange
            UserEntity userEntity = new UserEntity() { Username = "user11" };
            RegisterSchema schema = new RegisterSchema() { Username = "user11", Password = "Password123!" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);

            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ConflictResult>(result);
        }
        [Fact]
        public async Task RegisterAsync_WhenUnableToCreate_ShouldReturnIActionResultObjectWithStatusCode500()
        {
            //Arrange
            RegisterSchema schema = new RegisterSchema() { Username = "user11", Password = "Password123!" };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<UserEntity>());
            iUserRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(false);
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act
            IActionResult result = await authService.RegisterAsync(schema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            ObjectResult objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task RegisterAsync_WhenSchemaIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            AuthService authService = new AuthService(iJwtServiceMock.Object, iUserRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => authService.RegisterAsync(null));
        }
    }
}
