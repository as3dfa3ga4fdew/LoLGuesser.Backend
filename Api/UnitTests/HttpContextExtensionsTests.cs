using Api.Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class HttpContextExtensionsTests
    {
        [Fact]
        public void GetClaim_WhenJwtAndClaimExists_ShouldReturnValue()
        {
            //Arrange
            string claimType = "username";
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            Claim claim = context.GetClaim(claimType);

            //Assert
            Assert.Equal("string1", claim.Value);
        }
        [Fact]
        public void GetClaim_WhenInvalidClaim_ShouldReturnNull()
        {
            //Arrange
            string claimType = "username1";
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            Claim claim = context.GetClaim(claimType);

            //Assert
            Assert.Null(claim);
        }
        [Fact]
        public void GetClaim_WhenAuthorizationIsEmpty_ShouldReturnNull()
        {
            //Arrange
            HttpContext context = new DefaultHttpContext();

            //Act
            Claim claim = context.GetClaim(It.IsAny<string>());

            //Assert
            Assert.Null(claim);
        }
        [Fact]
        public void GetClaim_WhenAuthorizationIsNotSplittable_ShouldReturnNull()
        {
            //Arrange
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "";

            //Act
            Claim claim = context.GetClaim(It.IsAny<string>());

            //Assert
            Assert.Null(claim);
        }
        [Fact]
        public void GetClaim_WhenInvalidJwt_ShouldReturNull()
        {
            //Arrange
            HttpContext context = new DefaultHttpContext();
            context.Request.Headers.Authorization = "Bearer eJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            Claim claim = context.GetClaim(It.IsAny<string>());

            //Assert
            Assert.Null(claim);
        }
    }
}
