using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
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
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetChampionNames()).Returns(new List<string>().ToImmutableList<string>());
            
            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

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
            Mock<ILogger<IGameService>> iLoggerMock = new Mock<ILogger<IGameService>>(); 
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetChampionNames()).Throws<InvalidOperationException>();

            GameService gameService = new GameService(iDDragonCdnService.Object, iLoggerMock.Object);

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

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

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
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetRandomParsedChampion()).Returns(It.IsAny<ParsedChampion>());

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

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
            Mock<ILogger<IGameService>> iLoggerMock = new Mock<ILogger<IGameService>>();
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            iDDragonCdnService.Setup(x => x.GetRandomParsedChampion()).Throws<InvalidOperationException>();

            GameService gameService = new GameService(iDDragonCdnService.Object, iLoggerMock.Object);

            //Act
            IActionResult result = gameService.GetQuestion(QuestionType.Lore);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            ObjectResult objectResult = result as ObjectResult;

            Assert.Equal(objectResult.StatusCode, 500);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenValidAnswer_ShouldReturnIActionResultOkWithAnswerDto(QuestionType type)
        {
            //Arrange
            AnswerSchema schema = new AnswerSchema() { Id = It.IsAny<string>(), Type = type, Answer = "1" };
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = new ParsedChampion() { Name = "1" };
            iDDragonCdnService.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

            //Act
            IActionResult result = gameService.VerifyAnswer(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto data = okObjectResult.Value as AnswerDto;

            Assert.NotNull(data);
            Assert.True(data.Correct);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenInvalidAnswer_ShouldRetunrIActionResultOkWithAnswerDto(QuestionType type)
        {
            //Arrange
            AnswerSchema schema = new AnswerSchema() { Id = It.IsAny<string>(), Type = type, Answer = "1" };
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = new ParsedChampion() { Name = "2" };
            iDDragonCdnService.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

            //Act
            IActionResult result = gameService.VerifyAnswer(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto data = okObjectResult.Value as AnswerDto;

            Assert.NotNull(data);
            Assert.False(data.Correct);
        }
        [Fact]
        public void VerifyAnswer_WhenInvalidType_ShouldReturnIActionResultBadWithError()
        {
            //Arrange
            AnswerSchema schema = new AnswerSchema() { Id = It.IsAny<string>(), Type = (QuestionType)50, Answer = It.IsAny<string>() };
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

            //Act
            IActionResult result = gameService.VerifyAnswer(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);
            ErrorDto data = badRequestObjectResult.Value as ErrorDto;

            Assert.NotNull(data);
            Assert.Equal("Invalid type", data.Error);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenInvalidId_ShouldReturnIActionResultBadWithError(QuestionType type)
        {
            //Arrange
            AnswerSchema schema = new AnswerSchema() { Id = It.IsAny<string>(), Type = type, Answer = It.IsAny<string>() };
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = null;
            iDDragonCdnService.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(false);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(false);
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(false);

            GameService gameService = new GameService(iDDragonCdnService.Object, It.IsAny<ILogger<IGameService>>());

            //Act
            IActionResult result = gameService.VerifyAnswer(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<ErrorDto>(badRequestObjectResult.Value);
            ErrorDto data = badRequestObjectResult.Value as ErrorDto;

            Assert.NotNull(data);
            Assert.Equal("Invalid id", data.Error);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnser_WhenDDragonCdnServiceFails_ShouldReturnIActionResultObjectResultWith500StatusCode(QuestionType type)
        {
            AnswerSchema schema = new AnswerSchema() { Id = It.IsAny<string>(), Type = type, Answer = It.IsAny<string>() };
            Mock<ILogger<IGameService>> iLoggerMock = new Mock<ILogger<IGameService>>();
            Mock<IDDragonCdnService> iDDragonCdnService = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = null;
            iDDragonCdnService.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Throws<InvalidOperationException>();
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Throws<InvalidOperationException>();
            iDDragonCdnService.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Throws<InvalidOperationException>();
            
            GameService gameService = new GameService(iDDragonCdnService.Object, iLoggerMock.Object);

            //Act
            IActionResult result = gameService.VerifyAnswer(schema);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            ObjectResult objectResult = result as ObjectResult;

            Assert.Equal(objectResult.StatusCode, 500);
        }
    }
}
