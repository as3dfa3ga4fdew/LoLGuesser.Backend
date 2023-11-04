using Api.Controllers;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IUserRepository>());

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

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IUserRepository>());

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

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IUserRepository>());

            gameController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await gameController.GetQeuestionAsync(null);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task VerifyGuestAnswerAsync_ValidModelAndCorrectAnswer_ReturnsOkObjectResult()
        {
            //Arrange
            AnswerDto answerDto = new AnswerDto() { Correct = true, Score = 0 };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDto));

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IUserRepository>());

            //Act
            IActionResult result = await gameController.VerifyGuestAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task VerifyGuestAnswerAsync_InvalidModel_ReturnsBadRequest()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IUserRepository>());
            gameController.ModelState.AddModelError("", "");

            //Act
            IActionResult result = await gameController.VerifyGuestAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenValidModelAndAnswer_ShouldReturnOKAndUpdateScore()
        {
            //Arrange
            AnswerDto answerDtoMock = new AnswerDto() { Score = 1, Correct = true };
            UserEntity userEntityMock = new UserEntity() { Username = "user", Score = 0 };
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntityMock);
            iUserRepositoryMock.Setup(x => x.UpdateAsync(userEntityMock)).ReturnsAsync(true);
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDtoMock));

            GameController gameController = new GameController(iGameServiceMock.Object, iUserRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto answerDto = okObjectResult.Value as AnswerDto;

            Assert.Equal(1, userEntityMock.Score);
        }
        [Fact]
        public async Task VerifyAsnwerAsync_WhenValidModelAndIncorrectAnswer_ShouldReturnOkAndNotUpdateScore()
        {
            //Arrange
            AnswerDto answerDtoMock = new AnswerDto() { Score = 0, Correct = false };
            UserEntity userEntityMock = new UserEntity() { Username = "user", Score = 0 };
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntityMock);

            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDtoMock));

            GameController gameController = new GameController(iGameServiceMock.Object, iUserRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto answerDto = okObjectResult.Value as AnswerDto;

            Assert.Equal(0, userEntityMock.Score);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenClaimIsMissing_ShouldReturnBad()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IUserRepository>())
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenInvalidModel_ShouldReturnBad()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IUserRepository>());
            gameController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenInvalidValues_ShouldNotReturnOK()
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new BadRequestResult());

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IUserRepository>())
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenUserNoFound_ShouldReturnUnauthorized()
        {
            //Arrange
            AnswerDto answerDtoMock = new AnswerDto() { Score = 1, Correct = true };
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((UserEntity)null);
            
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDtoMock));

            GameController gameController = new GameController(iGameServiceMock.Object, iUserRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJ1c2VybmFtZSI6ImEifQ.6KyHj5iQrjqHMBWHCuvb2dbgoYJsN_m2MJXBsHfGhXU";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenUnableToUpdateUser_ShouldReturnStatusCode500()
        {
            //Arrange
            AnswerDto answerDtoMock = new AnswerDto() { Score = 1, Correct = true };
            UserEntity userEntityMock = new UserEntity() { Username = "user", Score = 0 };
            Mock<IUserRepository> iUserRepositoryMock = new Mock<IUserRepository>();
            iUserRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(userEntityMock);
            iUserRepositoryMock.Setup(x => x.UpdateAsync(userEntityMock)).ReturnsAsync(false);
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(new OkObjectResult(answerDtoMock));

            GameController gameController = new GameController(iGameServiceMock.Object, iUserRepositoryMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            gameController.HttpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0cmluZzEiLCJleHAiOjE2OTkwNDMyMzcsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.IVq55tf8aMPmHrm-q4dIX3qbHWefRvcD_wlu3cy6Z98";

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<StatusCodeResult>(result);

            StatusCodeResult statusCodeResult = result as StatusCodeResult;

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
