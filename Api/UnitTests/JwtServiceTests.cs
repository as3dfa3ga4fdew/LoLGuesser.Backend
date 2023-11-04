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
        public void Create_WhenSuccess_ShouldReturnString()
        {
            //Arrange
            Mock<IConfiguration> iConfigurationMock = new Mock<IConfiguration>();
            iConfigurationMock.SetupGet(x => x["Jwt:Key"]).Returns("xnbkVT8h22fXnmg7R98CN4GzoNduAZ2jdM7T5munshk8EnrAzNnpCsttJ13k1cup1LHqxko2y3C3XnbZf1viMomtUsABdeNvYHfG");
            iConfigurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("localhost");

            JwtService jwtService = new JwtService(iConfigurationMock.Object, It.IsAny<ILogger<IJwtService>>());

            //Act
            string jwt = jwtService.Create(It.IsAny<List<Claim>>());

            //Assert
            Assert.NotNull(jwt);
            Assert.IsType<string>(jwt);
        }
    }
}
