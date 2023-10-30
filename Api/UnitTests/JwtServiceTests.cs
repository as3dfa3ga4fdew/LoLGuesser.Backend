using Api.Services;
using Microsoft.Extensions.Configuration;
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
            List<Claim> claims = new List<Claim>() { new Claim("username","user1") };
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object);

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
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object);

            //Act
            string jwt = jwtService.Create(claims);

            //Assert
            Assert.NotNull(jwt);
            Assert.IsType<string>(jwt);
        }

        [Fact]
        public void Create_WhenMissingIConfigurationKeys_ShouldThrowInvalidOperationException()
        {
            //Arrange
            List<Claim> claims = null;
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["423443432"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object);

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => jwtService.Create(claims));
        }

        [Fact]
        public void JwtService_WhenMissingIConfigurationKeys_ShouldThrowInvalidOperationExceptionAndLogError()
        {
            //Arrange
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => new JwtService(iConfigurationMock.Object));
        }
    }
}
