using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        [Theory]
        [InlineData("id")]
        [InlineData("username")]
        public void TryGetClaim_WhenContextContainsJwt_ShouldReturnTrueAndOutClaim(string type)
        {
            //Arrange
            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns("Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZyIsImlkIjoiOTExNzVlNmYtZDcyZi00OWE1LTBkODktMDhkYmRkMzYwZTZlIiwiZXhwIjoxNjk5Mzc1MDExLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.SYkU08JQlNLHMdmjdaURy_RWYsnRAWqk06kooF_mLVk");

            JwtService jwtService = new JwtService(It.IsAny<IConfiguration>());

            //Act
            bool result = jwtService.TryGetClaim(httpContextMock.Object, type, out Claim claim);

            //Assert
            Assert.True(result);
            Assert.Equal(claim.Type, type);
        }

        [Fact]
        public void TryGetClaim_WhenAuthorizationIsNull_ShouldReturnFalseAndOutNull()
        {
            //Arrange
            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns((string)null);

            JwtService jwtService = new JwtService(It.IsAny<IConfiguration>());

            //Act
            bool result = jwtService.TryGetClaim(httpContextMock.Object, It.IsAny<string>(), out Claim claim);

            //Assert
            Assert.False(result);
            Assert.Null(claim);
        }

        [Fact]
        public void TryGetClaim_WhenInvalidAuthorizationString_ShouldReturnFalseAndOutNull()
        {
            //Arrange
            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns((string)null);

            JwtService jwtService = new JwtService(It.IsAny<IConfiguration>());
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns("BearereyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZyIsImlkIjoiOTExNzVlNmYtZDcyZi00OWE1LTBkODktMDhkYmRkMzYwZTZlIiwiZXhwIjoxNjk5Mzc1MDExLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.SYkU08JQlNLHMdmjdaURy_RWYsnRAWqk06kooF_mLVk");


            //Act
            bool result = jwtService.TryGetClaim(httpContextMock.Object, It.IsAny<string>(), out Claim claim);

            //Assert
            Assert.False(result);
            Assert.Null(claim);
        }

        [Fact]
        public void TryGetClaim_WhenInvalidJwtToken_ShouldReturnFalseAndOutNull()
        {
            //Arrange
            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns((string)null);

            JwtService jwtService = new JwtService(It.IsAny<IConfiguration>());
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns("Bearer eyJhbGciOiJIUzI1NiIsIn5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZyIsImlkIjoiOTExNzVlNmYtZDcyZi00OWE1LTBkODktMDhkYmRkMzYwZTZlIiwiZXhwIjoxNjk5Mzc1MDExLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.SYkU08JQlNLHMdmjdaURy_RWYsnRAWqk06kooF_mLVk");

            //Act
            bool result = jwtService.TryGetClaim(httpContextMock.Object, It.IsAny<string>(), out Claim claim);

            //Assert
            Assert.False(result);
            Assert.Null(claim);
        }

        [Fact]
        public void TryGetClaim_WhenTypeIsMissing_ShouldReturnFalseAndOutNull()
        {
            //Arrange
            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns((string)null);

            JwtService jwtService = new JwtService(It.IsAny<IConfiguration>());
            httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns("Bearer eyJhbGciOiJIUzI1NiIsIn5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZyIsImlkIjoiOTExNzVlNmYtZDcyZi00OWE1LTBkODktMDhkYmRkMzYwZTZlIiwiZXhwIjoxNjk5Mzc1MDExLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.SYkU08JQlNLHMdmjdaURy_RWYsnRAWqk06kooF_mLVk");

            //Act
            bool result = jwtService.TryGetClaim(httpContextMock.Object, "testtesttest", out Claim claim);

            //Assert
            Assert.False(result);
            Assert.Null(claim);
        }
    }
}
