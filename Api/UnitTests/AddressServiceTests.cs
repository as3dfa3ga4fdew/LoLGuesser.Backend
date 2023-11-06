using Api.Exceptions;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace UnitTests
{
    public class AddressServiceTests
    {
        [Fact]
        public void Validate_WhenValidSchema_ShouldReturnTrue()
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

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.True(isSuccess);
        }
        [Fact]
        public void Validate_WhenSchemaIsNull_ShouldReturnFalse()
        {
            //Arrange
            AddressSchema addressSchemaMock = null;

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")] // Empty string
        [InlineData("12")] // Below minimum length
        [InlineData("123456789012345678901234567890123456789012345678901")] // Above maximum length
        public void Validate_WhenInvalidFirstName_ShouldReturnFalse(string firstName)
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = firstName,
                LastName = "lastname",
                Street = "street",
                PostalCode = "12325",
                City = "city"
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")] // Empty string
        [InlineData("12")] // Below minimum length
        [InlineData("123456789012345678901234567890123456789012345678901")] // Above maximum length
        public void Validate_WhenInvalidLastName_ShouldReturnFalse(string lastName)
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = lastName,
                Street = "street",
                PostalCode = "12325",
                City = "city"
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")] // Empty string
        [InlineData("12")] // Below minimum length
        [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890111111111111111111111")] // Above maximum length
        public void Validate_WithInvalidStreet_ShouldReturnFalse(string street)
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = street,
                PostalCode = "12325",
                City = "city"
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")] // Empty string
        [InlineData("1234")] // Below minimum length
        [InlineData("12345678901")] // Above maximum length
        public void Validate_WithInvalidPostalCode_ShouldReturnFalse(string postalCode)
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = "street",
                PostalCode = postalCode,
                City = "city"
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")] // Empty string
        [InlineData("1")] // Below minimum length
        [InlineData("123456789012345678901234567890123456789012345678901")] // Above maximum length
        public void Validate_WithInvalidCity_ShouldReturnFalse(string city)
        {
            //Arrange
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = "street",
                PostalCode = "postalcode",
                City = city
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            bool isSuccess = addressService.Validate(addressSchemaMock);

            //Assert
            Assert.False(isSuccess);
        }

        [Fact]
        public void CreateEntity_WhenSucess_ShouldReturnAddressEntity()
        {
            //Arrange
            Guid userIdMock = Guid.NewGuid();
            AddressSchema addressSchemaMock = new AddressSchema()
            {
                Title = null,
                FirstName = "firstname",
                LastName = "lastname",
                Street = "street",
                PostalCode = "postalcode",
                City = "city"
            };

            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act
            AddressEntity entity = addressService.CreateEntity(addressSchemaMock, userIdMock);

            //Assert
            Assert.Equal(addressSchemaMock.FirstName, entity.FirstName);
        }

        [Fact]
        public async Task CreateAsync_WhenSuccess_ShouldReturnTrue()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool isSuccess = await addressService.CreateAsync(new AddressEntity());

            //Arrange
            Assert.True(isSuccess);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryFailed_ShouldReturnFalse()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(false);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool isSuccess = await addressService.CreateAsync(new AddressEntity());

            //Arrange
            Assert.False(isSuccess);
        }

        [Fact]
        public async Task CreateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            AddressService addressService = new AddressService(It.IsAny<IAddressRepository>());

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => addressService.CreateAsync(null));
        }

        [Fact]
        public async Task GetAllAsync_WhenSuccess_ShouldReturnEntityList()
        {
            //Arrange
            IEnumerable<AddressEntity> addressEntitiesMock = new List<AddressEntity>() { new AddressEntity() };
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<AddressEntity,bool>>>())).ReturnsAsync(addressEntitiesMock);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            IEnumerable<AddressEntity> result = await addressService.GetAllAsync(It.IsAny<Guid>());

            //Assert
            Assert.Equal(addressEntitiesMock.Count(), result.Count());
        }

        [Fact]
        public async Task RemoveAsync_WhenSuccess_ShouldReturnTrue()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync(new AddressEntity());
            iAddressRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool result = await addressService.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>());

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task RemoveAsync_WhenRepositoryFails_ShouldReturnFalse()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync(new AddressEntity());
            iAddressRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<AddressEntity>())).ReturnsAsync(false);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool result = await addressService.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task RemoveAsync_WhenAddressNotFound_ShouldThrowAddressNotFoundException()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync((AddressEntity)null);
            
            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<AddressNotFoundException>(() => addressService.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()));
        }
        [Fact]
        public async Task UpdateAsync_WhenSuccess_ShouldReturnTrue()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync(new AddressEntity());
            iAddressRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(true);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool result = await addressService.UpdateAsync(new AddressEntity());

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task UpdateAsync_WhenRepositoryFails_ShouldReturnFalse()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync(new AddressEntity());
            iAddressRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<AddressEntity>())).ReturnsAsync(false);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act
            bool result = await addressService.UpdateAsync(new AddressEntity());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            
            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => addressService.UpdateAsync(null));
        }
        [Fact]
        public async Task UpdateAsync_WhenAddressNotFound_ShouldAddressNotFoundException()
        {
            //Arrange
            Mock<IAddressRepository> iAddressRepositoryMock = new Mock<IAddressRepository>();
            iAddressRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<AddressEntity, bool>>>())).ReturnsAsync((AddressEntity)null);

            AddressService addressService = new AddressService(iAddressRepositoryMock.Object);

            //Act + Assert
            await Assert.ThrowsAsync<AddressNotFoundException>(() => addressService.UpdateAsync(new AddressEntity()));
        }
    }
}
