using Api.Controllers;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Models.Dtos;

namespace UnitTests
{
    public class AddressControllerTests
    {
        [Fact]
        public async Task CreateAsync_WhenSuccess_ShouldReturnCreatedAndAddressDto()
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = "street",
                PostalCode = "12325",
                City = "city"
            };
            AddressEntity addressEntityMock = new AddressEntity()
            {
                FirstName = "firstname"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.CreateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(true);
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(true);
            iAddressServiceMock.Setup(x => x.CreateEntity(It.IsAny<AddressSchema>(), It.IsAny<Guid>())).Returns(addressEntityMock);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.CreateAsync(addressSchemaMock);

            //Assert
            Assert.IsType<CreatedResult>(result);

            CreatedResult createdResult = result as CreatedResult;

            Assert.IsType<AddressDto>(createdResult.Value);

            AddressDto addressDto = createdResult.Value as AddressDto;

            Assert.Equal(addressSchemaMock.FirstName, addressDto.FirstName);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalidModelState_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), It.IsAny<IJwtService>());
            addressController.ModelState.AddModelError("", "");

            //Act
            IActionResult result = await addressController.CreateAsync(It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(1, errorDto.Code);
        }
        [Fact]
        public async Task CreateAsync_WhenClaimIsMissing_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = null;
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(false);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            addressController.HttpContext.Request.Headers.Authorization = "";

            //Act
            IActionResult result = await addressController.CreateAsync(It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(3, errorDto.Code);
        }
        [Fact]
        public async Task CreateAsync_WhenInvalidGuid_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.CreateAsync(It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(5, errorDto.Code);
        }
        [Fact]
        public async Task CreateAsync_WhenInvalidSchema_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(false);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.CreateAsync(It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(6, errorDto.Code);
        }
        [Fact]
        public async Task CreateAsync_WhenServiceFails_ShouldThrowException()
        {
            //Arrange
            AddressEntity addressEntityMock = new AddressEntity()
            {
                FirstName = "firstname"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.CreateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(false);
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(true);
            iAddressServiceMock.Setup(x => x.CreateEntity(It.IsAny<AddressSchema>(), It.IsAny<Guid>())).Returns(addressEntityMock);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(() => addressController.CreateAsync(It.IsAny<AddressSchema>()));
        }
        [Fact]
        public async Task GetAllAsync_WhenSuccess_ShouldReturnOkAndAddressDtos()
        {
            //Arrange
            IEnumerable<AddressEntity> addressEntitiesMock = new List<AddressEntity>() { new AddressEntity() };

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.GetAllAsync(It.IsAny<Guid>())).ReturnsAsync(addressEntitiesMock);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.GetAllAsync();

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsAssignableFrom<IEnumerable<AddressDto>>(okObjectResult.Value);
            IEnumerable<AddressDto> addressEntities = okObjectResult.Value as IEnumerable<AddressDto>;

            Assert.Equal(addressEntitiesMock.Count(), addressEntities.Count());
        }
        [Fact]
        public async Task GetAllAsync_WhenClaimIsMissing_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = null;
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(false);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.GetAllAsync();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(3, errorDto.Code);
        }
        [Fact]
        public async Task GetAllAsync_WhenInvalidGuid_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.GetAllAsync();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(5, errorDto.Code);
        }
        [Fact]
        public async Task DeleteAsync_WhenValidId_ShouldReturnNoContent()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.DeleteAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_WhenInvalidModelState_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), It.IsAny<IJwtService>());
            addressController.ModelState.AddModelError("", "");

            //Act
            IActionResult result = await addressController.DeleteAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(1, errorDto.Code);
        }

        [Fact]
        public async Task DeleteAsync_WhenClaimIsMissing_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = null;
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(false);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.DeleteAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(3, errorDto.Code);
        }
        [Fact]
        public async Task DeleteAsync_WhenInvalidGuid_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.DeleteAsync(It.IsAny<Guid>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(5, errorDto.Code);
        }

        [Fact]
        public async Task DeleteAsync_WhenServiceFails_ShouldThrowException()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(() => addressController.DeleteAsync(It.IsAny<Guid>()));
        }

        [Fact]
        public async Task UpdateAsync_WhenValidIdAndSchema_ShouldReturnOkWithAddressDto()
        {
            //Arrange
            Guid addressId = Guid.NewGuid();
            AddressEntity addressEntityMock = new AddressEntity()
            {
                FirstName = "firstname"
            };
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = "street",
                PostalCode = "12325",
                City = "city"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(true);
            iAddressServiceMock.Setup(x => x.CreateEntity(It.IsAny<AddressSchema>(), It.IsAny<Guid>())).Returns(addressEntityMock);
            iAddressServiceMock.Setup(x => x.UpdateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.PutAsync(addressId, addressSchemaMock);

            //Assert
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AddressDto>(okObjectResult.Value);

            AddressDto addressDto = okObjectResult.Value as AddressDto;

            Assert.Equal(addressSchemaMock.FirstName, addressDto.FirstName);
        }
        [Fact]
        public async Task UpdateAsync_WhenInvalidModelState_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), It.IsAny<IJwtService>());
            addressController.ModelState.AddModelError("", "");

            //Act
            IActionResult result = await addressController.PutAsync(It.IsAny<Guid>(),It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(1, errorDto.Code);
        }
        [Fact]
        public async Task UpdateAsync_WhenClaimIsMissing_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = null;
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(false);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            addressController.HttpContext.Request.Headers.Authorization = "";

            //Act
            IActionResult result = await addressController.PutAsync(It.IsAny<Guid>(), It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(3, errorDto.Code);
        }
        [Fact]
        public async Task UpdateAsync_WhenInvalidGuid_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            AddressController addressController = new AddressController(It.IsAny<IAddressService>(), iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.PutAsync(It.IsAny<Guid>(), It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(5, errorDto.Code);
        }
        [Fact]
        public async Task UpdateAsync_WhenInvalidSchema_ShouldReturnBadRequestWithErrorDto()
        {
            //Arrange
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(false);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act
            IActionResult result = await addressController.PutAsync(It.IsAny<Guid>(), It.IsAny<AddressSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);

            ErrorDto errorDto = badRequestObjectResult.Value as ErrorDto;

            Assert.Equal(6, errorDto.Code);
        }
        [Fact]
        public async Task UpdateAsync_WhenServiceFails_ShouldThrowException()
        {
            //Arrange
            AddressEntity addressEntityMock = new AddressEntity()
            {
                FirstName = "firstname"
            };
            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IAddressService> iAddressServiceMock = new Mock<IAddressService>();
            iAddressServiceMock.Setup(x => x.UpdateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(false);
            iAddressServiceMock.Setup(x => x.Validate(It.IsAny<AddressSchema>())).Returns(true);
            iAddressServiceMock.Setup(x => x.CreateEntity(It.IsAny<AddressSchema>(), It.IsAny<Guid>())).Returns(addressEntityMock);

            AddressController addressController = new AddressController(iAddressServiceMock.Object, iJwtServiceMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<Exception>(() => addressController.PutAsync(It.IsAny<Guid>(), It.IsAny<AddressSchema>()));
        }
    }
}
