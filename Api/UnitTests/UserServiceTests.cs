using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public void ValidateUsername_WhenValidUsername_ShouldReturnTrue()
        {
            //Arrange
            string username = "username";
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.ValidateUsername(username);

            //Assert
            Assert.True(result);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("short")] //Too short
        [InlineData("aaaaaaaaaaaaaaaaa")] //Too long
        [InlineData("usernameö")] //Dissllowed characters
        public void ValidateUsername_WhenInvalidUsername_ShouldReturnFalse(string username)
        {
            //Arrange
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.ValidateUsername(username);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void ValidatePassword_WhenValidPassword_ShouldReturnTrue()
        {
            //Arrange
            string password = "Password123@";
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.ValidatePassword(password);

            //Assert
            Assert.True(result);
        }
        [Theory]
        [InlineData(null)]                // Null password
        [InlineData("Short1!")]           // Too short
        [InlineData("VeryLongPassword123!VeryLongPassword123!VeryLongPassword123!")] // Too long
        [InlineData("NoDigitsOrSpecialChars")] // Missing digits and special characters
        [InlineData("UpperCaseOnly")]           // Missing lowercase characters
        [InlineData("Lowercase123")]            // Missing uppercase characters
        [InlineData("SpecialChars@")]           // Missing numbers
        [InlineData("NoUpperCaseOrSpecial123")] // Missing uppercase characters and special characters
        [InlineData("usernameö")] //Dissllowed characters
        public void ValidatePassword_WithInvalidPassword_ShouldReturnFalse(string password)
        {
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.ValidatePassword(password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenValidLoginSchema_ShouldReturnTrue()
        {
            //Arrange
            LoginSchema loginSchema = new LoginSchema() { Username = "Username", Password = "Password123@" };
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.Validate(loginSchema);

            //Assert
            Assert.True(result);
        }
        [Theory]
        [InlineData("use","password123@")] //Invalid username
        [InlineData("username","password")] //Invalid password
        public void Validate_WhenInvalidLoginSchema_ShouldReturnFalse(string username, string password)
        {
            //Arrange
            LoginSchema loginSchema = new LoginSchema() { Username = username, Password = password };
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.Validate(loginSchema);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenValidRegisterSchema_ShouldReturnTrue()
        {
            //Arrange
            RegisterSchema loginSchema = new RegisterSchema() { Username = "Username", Password = "Password123@" };
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.Validate(loginSchema);

            //Assert
            Assert.True(result);
        }
        [Theory]
        [InlineData("use", "password123@")] //Invalid username
        [InlineData("username", "password")] //Invalid password
        public void Validate_WhenInvalidRegisterSchema_ShouldReturnFalse(string username, string password)
        {
            //Arrange
            RegisterSchema loginSchema = new RegisterSchema() { Username = username, Password = password };
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.Validate(loginSchema);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenSchemaIsNull_ShouldReturnFalse()
        {
            //Arrange
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = userService.Validate(It.IsAny<Type>());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenInvalidSchemaType_ShouldThrowException()
        {
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act + Assert
            Assert.Throws<Exception>(() => userService.Validate(new { }));
        }
        [Fact]
        public async Task GetByUsernameAsync_WhenUserExists_ShouldReturnUserEntity()
        {
            //Arrange
            UserEntity userEntity = new UserEntity() { Username = "username" };
            Mock<IUserRepository> iUserRepository = new Mock<IUserRepository>();
            iUserRepository.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntity);

            UserService userService = new UserService(iUserRepository.Object);

            //Act
            UserEntity result = await userService.GetByUsernameAsync("");

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userEntity.Username, result.Username);
        }
        [Fact]
        public async Task GetByUsernameAsync_WhenUserMissing_ShouldReturnNull()
        {
            //Arrange
            Mock<IUserRepository> iUserRepository = new Mock<IUserRepository>();
            iUserRepository.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);

            UserService userService = new UserService(iUserRepository.Object);

            //Act
            UserEntity result = await userService.GetByUsernameAsync("");

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetByUsernameAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.GetByUsernameAsync(It.IsAny<string>()));
        }
        [Fact]
        public async Task CreateAsync_WhenSuccess_ShouldReturnEntity()
        {
            //Arrange
            UserEntity userEntity = new UserEntity() { Username = "username" };
            Mock<IUserRepository> iUserRepository = new Mock<IUserRepository>();
            iUserRepository.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

            UserService userService = new UserService(iUserRepository.Object);

            //Act
            bool result = await userService.CreateAsync(userEntity);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task CreateAsync_WhenRepositoryFails_ShouldReturnFalse()
        {
            //Arrange
            UserEntity userEntity = new UserEntity() { Username = "username" };
            Mock<IUserRepository> iUserRepository = new Mock<IUserRepository>();
            iUserRepository.Setup(x => x.CreateAsync(It.IsAny<UserEntity>())).ReturnsAsync(false);

            UserService userService = new UserService(iUserRepository.Object);

            //Act
            bool result = await userService.CreateAsync(userEntity);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task CreateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.CreateAsync((UserEntity)null));
        }
        [Fact]
        public async Task VerifyPasswordAsync_WhenEqual_ShouldReturnTrue()
        {
            //Arrange
            string password = "Password123@";
            string hash = "$2a$04$lO4j/2.QBGllhf1fSZqwhORJ3DzfJfBjiHEEgGDr8KxQnPbVHXpPq";
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = await userService.VerifyPasswordAsync(password, hash);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task VerifyPasswordAsync_WhenNotEqualPassword_ShouldReturnFalse()
        {
            //Arrange
            string password = "Password123";
            string hash = "$2a$04$lO4j/2.QBGllhf1fSZqwhORJ3DzfJfBjiHEEgGDr8KxQnPbVHXpPq";
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            bool result = await userService.VerifyPasswordAsync(password, hash);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task HashPasswordAsync_WhenSuccess_ShouldReturnHashedString()
        {
            //Arrange
            string password = "Password123@";
            UserService userService = new UserService(It.IsAny<IUserRepository>());

            //Act
            string result = await userService.HashPasswordAsync(password, 4);

            //Assert
            Assert.Contains("$2a$04$", result);
        }
       
    }
}
