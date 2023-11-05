using Api.Models.Dtos;
using Api.Models.Schemas;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTests
{
    public class AuthTests
    {
        RestClient client = new RestClient(new RestClientOptions() { BaseUrl = new Uri("https://localhost:5000/") });

        [Fact]
        public async Task Login_WhenValidUsernameAndPassword_ShouldReturnOkAndLoginDto()
        {
            //Arrange
            LoginSchema schema = new LoginSchema() { Username = "string", Password = "Password123@"};

            RestRequest request = new RestRequest();

            var response = await client.PostJsonAsync<LoginSchema,LoginDto>("auth/login", schema);

            //Act
            


        }
    }
}
