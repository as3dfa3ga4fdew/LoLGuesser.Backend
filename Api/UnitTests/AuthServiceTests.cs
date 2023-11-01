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
            UserEntity userEntity = new UserEntity() { Id = Guid.NewGuid(), Username = "user12", Password = "$2a$12$knXKz6yYJ/SvcbICOwUEn.qtU6fI397rLjEqRcoPccT48ASB4grra", Score = 0 };
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
    }
}
