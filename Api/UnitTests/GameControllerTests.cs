using Api.Controllers;
using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class GameControllerTests
    {
        [Fact]
        public async Task GetChampionNamesAsync_WhenSuccess_ShouldReturnIActionResultOkWithImmutableList()
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.GetChampionNames()).Returns(new OkObjectResult(new List<string>().ToImmutableList()));

            GameController gameController = new GameController(iGameServiceMock.Object);

            //Act
            IActionResult result = await gameController.GetChampionNamesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsAssignableFrom<IImmutableList<string>>(okObjectResult.Value);
        }

        [Fact]
        public async Task GetQuestionAsync_WhenValidSchema_ShouldReturnIActionResultOKWithQuestionDto()
        {
            //Arrange
            QuestionSchema questionSchema = new QuestionSchema() { Type = QuestionType.Lore };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.GetQuestion(It.IsAny<QuestionType>())).Returns(new OkObjectResult(new QuestionDto()));

            GameController gameController = new GameController(iGameServiceMock.Object);

            //Act
            IActionResult result = await gameController.GetQeuestionAsync(questionSchema);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.IsType<QuestionDto>(okObjectResult.Value);
        }

        [Fact]
        public async Task GetQuestionAsync_WhenInvalidSchema_ShouldReturnIActionResultBad()
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();

            GameController gameController = new GameController(iGameServiceMock.Object);

            gameController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await gameController.GetQeuestionAsync(null);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task VerifyGuestAnswerAsync_WhenValidAnswer_ShouldReturIActionResultOk()
        {
            //Arrange
            AnswerDto answerDto = new AnswerDto() { Correct = true, Score = 0 };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDto));

            GameController gameController = new GameController(iGameServiceMock.Object);

            //Act
            IActionResult result = await gameController.VerifyGuestAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task VerifyGuestAnswerAsync_WhenInvalidSchema_ShouldReturnIActionResultBad()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>());
            gameController.ModelState.AddModelError("", "");

            //Act
            IActionResult result = await gameController.VerifyGuestAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
