using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Services;
using Api.Services.Interfaces;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class GameServiceTests
    {
        [Fact]
        public void GetChampionNames_WhenSuccess_ShouldReturnIActionResultOkWithImmutableList()
        {
            //Arrange
            Mock<ILogger<GameService>> loggerMock = new Mock<ILogger<GameService>>();
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetChampionNames()).Returns(new List<string>().ToImmutableList<string>());
            
            GameService gameService = new GameService(iDDragonCdnService.Object, loggerMock.Object);

            //Act
            IActionResult result = gameService.GetChampionNames();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsAssignableFrom<IImmutableList<string>>(okObjectResult.Value);
        }

        [Fact]
        public void GetChampionNames_WhenDDragonCdnServiceFails_ShouldReturnIActionResultObjectResultWith500StatusCode()
        {
            //Arrange
            Mock<ILogger<GameService>> loggerMock = new Mock<ILogger<GameService>>();
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetChampionNames()).Throws<InvalidOperationException>();

            GameService gameService = new GameService(iDDragonCdnService.Object, loggerMock.Object);

            //Act
            IActionResult result = gameService.GetChampionNames();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            ObjectResult objectResult = result as ObjectResult;

            Assert.Equal(objectResult.StatusCode, 500);
        }
        [Theory]
        [InlineData(QuestionType.Lore)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Spell)]
        public void GetQuesiton_WhenValidQuestionType_ShouldReturnIActionResultOKWithQuestionDto(QuestionType questionType)
        {
            //Arrange
            Mock<ILogger<GameService>> loggerMock = new Mock<ILogger<GameService>>();
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetRandomParsedChampion())
                .Returns(
                    new ParsedChampion() 
                    { 
                        Name = "Rengar",
                        RedactedLore = new KeyValuePair<string, string>("1",""),
                        SplashArtUrls = new KeyValuePair<string, List<string>>("2", new List<string>() { "url" }),
                        SpellUrls = new KeyValuePair<string, List<string>>("3", new List<string>() { "url" })
                    }
                );

            GameService gameService = new GameService(iDDragonCdnService.Object, loggerMock.Object);

            //Act
            IActionResult result = gameService.GetQuestion(questionType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<QuestionDto>(okObjectResult.Value);
            QuestionDto data = okObjectResult.Value as QuestionDto;

            Assert.NotNull(data);
            Assert.Equal(questionType, data.Type);
        }
        [Fact]
        public void GetQuestion_WhenInvalidQuestionType_ShouldReturnIActionResultBad()
        {
            //Arrange
            QuestionType invalidType = (QuestionType)100;
            Mock<ILogger<GameService>> loggerMock = new Mock<ILogger<GameService>>();

            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetRandomParsedChampion()).Returns(It.IsAny<ParsedChampion>());

            GameService gameService = new GameService(iDDragonCdnService.Object, loggerMock.Object);

            //Act
            IActionResult result = gameService.GetQuestion(invalidType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void GetQuestion_WhenDDragonCdnServiceFails_ShouldReturnIActionResultObjectResultWith500StatusCode()
        {
            //Arrange
            Mock<ILogger<GameService>> loggerMock = new Mock<ILogger<GameService>>();

            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetRandomParsedChampion()).Throws<InvalidOperationException>();

            GameService gameService = new GameService(iDDragonCdnService.Object, loggerMock.Object);

            //Act
            IActionResult result = gameService.GetQuestion(QuestionType.Lore);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            ObjectResult objectResult = result as ObjectResult;

            Assert.Equal(objectResult.StatusCode, 500);
        }
    }
}
