using Api.Clients;
using Api.Clients.Interfaces;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace UnitTests
{
    public class DDragonCdnClientTests
    {
        [Fact]
        public async Task GetVersionsAsync_WhenCdnCallSucceeds_ShouldReturnAListOfStrings()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[\"v21\"]")
            };

            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);


            IDDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string> result = await dDragonCdnClient.GetVersionsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetVersionsAsync_WhenCdnCallFails_ShouldReturnNull()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException());

            var httpClient = new HttpClient(handlerMock.Object);

            IDDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string> result = await dDragonCdnClient.GetVersionsAsync();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetVersionsAsync_WhenResponseCannotBeDeserialized_ShouldReturnNull()
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("error")
            };

            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            IDDragonCdnClient dDragonCdnClient = new DDragonCdnClient(httpClient);

            //Act
            List<string> result = await dDragonCdnClient.GetVersionsAsync();

            //Assert
            Assert.Null(result);
        }
    }
}