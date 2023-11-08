using Api.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public class AddressEndpointTests
    {
        [Fact]
        public async Task Get_Addresses_WhenValidSchema_ShouldReceiveListOfAddresses()
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

            //Login

            //Add address

            //Act

            //Assert
        }
    }
}
