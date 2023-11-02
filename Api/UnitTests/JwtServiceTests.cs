using Api.Services;
using Api.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class JwtServiceTests
    {
        [Fact]
        public void Create_WhenValidClaimsProvided_ShouldReturnString()
        {
            //Arrange
            List<Claim> claims = new List<Claim>() { new Claim("username", "user1") };
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            Mock<ILogger<IJwtService>> loggerMock = new Mock<ILogger<IJwtService>>();
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object, loggerMock.Object);

            //Act
            string jwt = jwtService.Create(claims);

            //Assert
            Assert.NotNull(jwt);
            Assert.IsType<string>(jwt);
        }

        [Fact]
        public void Create_WhenClaimsIsNull_ShouldReturnString()
        {
            //Arrange
            List<Claim> claims = null;
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            Mock<ILogger<IJwtService>> loggerMock = new Mock<ILogger<IJwtService>>();
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object, loggerMock.Object);

            //Act
            string jwt = jwtService.Create(claims);

            //Assert
            Assert.NotNull(jwt);
            Assert.IsType<string>(jwt);
        }

        [Fact]
        public void JwtService_WhenMissingIConfigurationKeys_ShouldThrowInvalidOperationExceptionAndLogError()
        {
            //Arrange
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            Mock<ILogger<IJwtService>> iLoggerMock = new Mock<ILogger<IJwtService>>();
            iLoggerMock.Setup(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((@object, @type) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ));

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => new JwtService(iConfigurationMock.Object, iLoggerMock.Object));
            iLoggerMock.Verify(
                logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((@object, @type) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
