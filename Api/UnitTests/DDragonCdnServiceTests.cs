using Api.Clients;
using Api.Clients.Interfaces;
using Api.Models.DDragonClasses;
using Api.Services;
using Api.Services.Interfaces;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;

namespace UnitTests
{
    public class DDragonCdnServiceTests
    {

        [Fact]
        public async Task UpdateAsync_WhenSuccessful_ShouldSetRoot()
        {
            Root mockRoot = new Root() { Version = "13.20.1" };
            Mock<IDDragonCdnClient> mockDDragonCdnClient = new Mock<IDDragonCdnClient>();
            mockDDragonCdnClient.Setup(client => client.GetVersionsAsync()).ReturnsAsync(new List<string> { "13.20.1" });
            mockDDragonCdnClient.Setup(client => client.GetDataAsync("13.20.1")).ReturnsAsync(mockRoot);

            IDDragonCdnService dDragonCdnService = new DDragonCdnService(mockDDragonCdnClient.Object);

            // Act
            await dDragonCdnService.UpdateAsync();

            // Assert
            var privateRoot = GetPrivateField<Root>(dDragonCdnService, "_root");
            Assert.NotNull(privateRoot);
            Assert.Same(mockRoot, privateRoot);
        }

        [Fact]
        public async Task UpdateAsync_WhenGettingNullVersion_ShouldNotSetRoot()
        {
            Root mockRoot = new Root() { Version = "13.20.1" };
            Mock<IDDragonCdnClient> mockDDragonCdnClient = new Mock<IDDragonCdnClient>();
            mockDDragonCdnClient.Setup(client => client.GetVersionsAsync()).ReturnsAsync((List<string>)null);
            mockDDragonCdnClient.Setup(client => client.GetDataAsync("13.20.1")).ReturnsAsync(mockRoot);

            IDDragonCdnService dDragonCdnService = new DDragonCdnService(mockDDragonCdnClient.Object);

            // Act
            await dDragonCdnService.UpdateAsync();


            //Assert
            var privateRoot = GetPrivateField<Root>(dDragonCdnService, "_root");
            Assert.Null(privateRoot);
        }

        [Fact]
        public async Task UpdateAsync_WhenGettingNullData_ShouldNotSetRoot()
        {
            
            Mock<IDDragonCdnClient> mockDDragonCdnClient = new Mock<IDDragonCdnClient>();
            mockDDragonCdnClient.Setup(client => client.GetVersionsAsync()).ReturnsAsync(new List<string>() { "13.20.1" });
            mockDDragonCdnClient.Setup(client => client.GetDataAsync("13.20.1")).ReturnsAsync((Root)null);

            IDDragonCdnService dDragonCdnService = new DDragonCdnService(mockDDragonCdnClient.Object);

            // Act
            await dDragonCdnService.UpdateAsync();

            //Assert
            var privateRoot = GetPrivateField<Root>(dDragonCdnService, "_root");
            Assert.Null(privateRoot);
        }

     
        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            obj.GetType().GetProperty(fieldName).SetValue(obj, value);
        }
        private static T GetPrivateField<T>(object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)fieldInfo.GetValue(obj);
        }
    }
}
