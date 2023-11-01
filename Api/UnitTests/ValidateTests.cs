using Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ValidateTests
    {
        [Fact]
        public void Username_WhenUsernameIsValid_ShouldReturnTrue()
        {
            //Arrange
            string username = "Username1";

            //Act
            bool result = Validate.Username(username);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Username_WhenNullUsername_ShouldReturnFalse()
        {
            //Arrange
            string username = null;

            //Act
            bool result = Validate.Username(username);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Username_WhenShortUsername_ShouldReturnFalse()
        {
            //Arrange
            string username = "Short";

            //Act
            bool result = Validate.Username(username);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Username_WhenLongUsername_ShouldReturnFalse()
        {
            // Arrange
            var username = "ThisIsAVeryLongUsername";

            // Act
            var result = Validate.Username(username);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Username_WhenInvalidCharacters_ShouldReturnFalse()
        {
            // Arrange
            var username = "ThisIs!";

            // Act
            var result = Validate.Username(username);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Password_WhenValidPassword_ShouldReturnTrue()
        {
            // Arrange
            var password = "ValidPassword123!";

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void Password_WhenNullPassword_ShouldReturnFalse()
        {
            // Arrange
            string password = null;

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Password_WhenShortPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "Short1!";

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Password_WhenLongPassword_ShouldReturnFalse()
        {
            // Arrange
            var password = "ThisIsAVeryLongPassword123!" + new string('A', 40);

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Password_WhenMissingCharacterSets_ShouldReturnFalse()
        {
            // Arrange
            var password = "OnlyLetters";

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void Password_WhenInvalidCharacter_ShouldReturnFalse()
        {
            // Arrange
            var password = "Invalid@Password123ö";

            // Act
            var result = Validate.Password(password);

            // Assert
            Assert.False(result);
        }
    }
}
