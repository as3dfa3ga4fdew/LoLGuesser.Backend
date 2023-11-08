using Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models.Dtos;

namespace UnitTests
{
    public class StatusDtoTests
    {
        [Theory]
        [InlineData(ErrorType.Unexcepted)]
        [InlineData(ErrorType.InvalidModel)]
        [InlineData(ErrorType.NotFound)]
        [InlineData(ErrorType.MissingIdClaim)]
        [InlineData(ErrorType.MissingUsernameClaim)]
        [InlineData(ErrorType.InvalidGuid)]
        [InlineData(ErrorType.InvalidSchema)]
        [InlineData(ErrorType.InvalidId)]
        [InlineData(ErrorType.InvalidCredentials)]
        [InlineData(ErrorType.UsernameAlreadyTaken)]
        public void StatusDto_WhenErrorTypeExists_ShouldSet(ErrorType type)
        {
            //Arrange + Act
            StatusDto statusDto = new StatusDto(type);

            //Assert
            Assert.Equal((int)type, statusDto.Code);
        }
        [Fact]
        public void StatusDto_WhenInvalidType_ShouldSetEmpty()
        {
            //Arrange + Act
            StatusDto statusDto = new StatusDto((ErrorType)100);

            //Assert
            Assert.Equal("", statusDto.Message);
        }
    }
}
