using Api.Controllers;
using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class GameControllerTests
    {
        [Fact]
        public void GetChampionNames_WhenSuccess_ShouldReturnOkWithStrings()
        {
            //Arrange
            IImmutableList<string> names = new List<string>() { "" }.ToImmutableList();
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.GetChampionNames()).Returns(names);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetChampionNames();

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsAssignableFrom<IImmutableList<string>>(okObjectResult.Value);
            IImmutableList<string> iImmutableList = okObjectResult.Value as IImmutableList<string>;

            Assert.Equal(names.Count, iImmutableList.Count);
        }
        [Fact]
        public void GetQeuestion_WhenValidSchema_ShouldReturnOkWithQuestionDto()
        {
            //Arrange
            QuestionSchema questionSchemaMock = new QuestionSchema()
            {
                Type = QuestionType.Lore
            };
            QuestionDto questionDtoMock = new QuestionDto()
            {
                Id = "",
                Type = QuestionType.Lore,
                Value = ""
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(questionSchemaMock)).Returns(true);
            iGameServiceMock.Setup(x => x.GetQuestion(It.IsAny<QuestionType>())).Returns(questionDtoMock);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetQeuestion(questionSchemaMock);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<QuestionDto>(okObjectResult.Value);
            QuestionDto questionDto = okObjectResult.Value as QuestionDto;

            Assert.Equal(questionSchemaMock.Type, questionDto.Type);
        }
        [Fact]
        public void GetQuestion_WhenInvalidModelState_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IJwtService>(), It.IsAny<IUserService>());
            gameController.ModelState.AddModelError("","");

            //Act
            IActionResult result =  gameController.GetQeuestion(It.IsAny<QuestionSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(1, statusDto.Code);
        }
        [Fact]
        public void GetQuestion_WhenInvalidSchema_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            QuestionSchema questionSchemaMock = new QuestionSchema()
            {
                Type = QuestionType.Lore
            };
            QuestionDto questionDtoMock = new QuestionDto()
            {
                Id = "",
                Type = QuestionType.Lore,
                Value = ""
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(questionSchemaMock)).Returns(false);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetQeuestion(questionSchemaMock);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(6, statusDto.Code);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenCorrectAnswerAndNotAuthed_ShouldReturnOkWithAnswerDto()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>())
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal((new ClaimsIdentity()))
                    }
                }
            };

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(answerSchema);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto answerDto = okObjectResult.Value as AnswerDto;

            Assert.True(answerDto.Correct);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenCorrectAnswerAndAuthed_ShouldReturnOkWithAnswerDto()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "",
                Score = 0
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(userEntity);
            iUserServiceMock.Setup(x => x.UpdateAsync(userEntity)).ReturnsAsync(true);

            GameController gameController = new GameController(iGameServiceMock.Object, iJwtServiceMock.Object, iUserServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username")
                        }, "someAuthTypeName"))
                    }
                }
            };

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(answerSchema);

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto answerDto = okObjectResult.Value as AnswerDto;

            Assert.True(answerDto.Correct);
            Assert.Equal(1,userEntity.Score);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenInvalidModelState_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IJwtService>(), It.IsAny<IUserService>());
            gameController.ModelState.AddModelError("","");

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(1, statusDto.Code);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenInvalidSchema_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(false);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(6, statusDto.Code);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenInCorrectAnswer_ShouldReturnOkWithAnswerDto()
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(It.IsAny<AnswerSchema>())).Returns(false);
            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(It.IsAny<AnswerSchema>());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<AnswerDto>(okObjectResult.Value);
            AnswerDto answerDto = okObjectResult.Value as AnswerDto;

            Assert.False(answerDto.Correct);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenClaimIsMissing_ShouldReturnBadRequestWithStatusDto()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "",
                Score = 0
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(false);

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();

            GameController gameController = new GameController(iGameServiceMock.Object, iJwtServiceMock.Object, iUserServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username")
                        }, "someAuthTypeName"))
                    }
                }
            };

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(answerSchema);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(3, statusDto.Code);
        }

        [Fact]
        public async Task VerifyAnswerAsync_WhenInvalidGuid_ShouldReturnBadRequestWithStatusto()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "",
                Score = 0
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd36");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();

            GameController gameController = new GameController(iGameServiceMock.Object, iJwtServiceMock.Object, iUserServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username")
                        }, "someAuthTypeName"))
                    }
                }
            };

            //Act
            IActionResult result = await gameController.VerifyAnswerAsync(answerSchema);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(5, statusDto.Code);
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenUserIsMissing_ShouldThrowException()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "",
                Score = 0
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);
            
            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((UserEntity)null);

            GameController gameController = new GameController(iGameServiceMock.Object, iJwtServiceMock.Object, iUserServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username")
                        }, "someAuthTypeName"))
                    }
                }
            };

            //Act
            await Assert.ThrowsAsync<Exception>(() => gameController.VerifyAnswerAsync(answerSchema));
        }
        [Fact]
        public async Task VerifyAnswerAsync_WhenUpdateFailed_ShouldThrowException()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "answer"
            };
            UserEntity userEntity = new UserEntity()
            {
                Username = "",
                Score = 0
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.Validate(It.IsAny<AnswerSchema>())).Returns(true);
            iGameServiceMock.Setup(x => x.VerifyAnswer(answerSchema)).Returns(true);

            Mock<IJwtService> iJwtServiceMock = new Mock<IJwtService>();
            Claim claim = new Claim("id", "91175e6f-d72f-49a5-0d89-08dbdd360e6e");
            iJwtServiceMock.Setup(x => x.TryGetClaim(It.IsAny<HttpContext>(), It.IsAny<string>(), out claim)).Returns(true);

            Mock<IUserService> iUserServiceMock = new Mock<IUserService>();
            iUserServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(userEntity);
            iUserServiceMock.Setup(x => x.UpdateAsync(userEntity)).ReturnsAsync(false);

            GameController gameController = new GameController(iGameServiceMock.Object, iJwtServiceMock.Object, iUserServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "username")
                        }, "someAuthTypeName"))
                    }
                }
            };

            //Act
            await Assert.ThrowsAsync<Exception>(() => gameController.VerifyAnswerAsync(answerSchema));
        }

        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void GetChampionByTypeAndAndId_WhenValidTypeAndId_ShouldReturnOKWithParsedChampion(QuestionType type)
        {
            //Arrange
            ParsedChampion parsedChampionMock = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("1", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("1", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("1", new List<string>() { "" })
            };
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.GetParsedChampionById(It.IsAny<QuestionType>(), It.IsAny<string>())).Returns(parsedChampionMock);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetChampionByTypeAndAndId(type, Guid.NewGuid());

            //Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult okObjectResult = result as OkObjectResult;

            Assert.IsType<ParsedChampion>(okObjectResult.Value);
            ParsedChampion parsedChampion = okObjectResult.Value as ParsedChampion;

            Assert.Equal(parsedChampionMock.Name, parsedChampion.Name);
        }

        [Fact]
        public void GetChampionByTypeAndAndId_WhenInvalidType_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetChampionByTypeAndAndId((QuestionType)100, Guid.NewGuid());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(6, statusDto.Code);
        }


        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void GetChampionByTypeAndAndId_WhenInvalidId_ShouldReturnBadWithStatusDto(QuestionType type)
        {
            //Arrange
            Mock<IGameService> iGameServiceMock = new Mock<IGameService>();
            iGameServiceMock.Setup(x => x.GetParsedChampionById(It.IsAny<QuestionType>(), It.IsAny<string>())).Returns((ParsedChampion)null);

            GameController gameController = new GameController(iGameServiceMock.Object, It.IsAny<IJwtService>(), It.IsAny<IUserService>());

            //Act
            IActionResult result = gameController.GetChampionByTypeAndAndId(type, Guid.NewGuid());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(7, statusDto.Code);
        }

        [Fact]
        public void GetParsedChampionById_WhenInvalidModelState_ShouldReturnBadWithStatusDto()
        {
            //Arrange
            GameController gameController = new GameController(It.IsAny<IGameService>(), It.IsAny<IJwtService>(), It.IsAny<IUserService>());
            gameController.ModelState.AddModelError("","");
            //Act
            IActionResult result = gameController.GetChampionByTypeAndAndId((QuestionType)100, Guid.NewGuid());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;

            Assert.IsType<StatusDto>(badRequestObjectResult.Value);
            StatusDto statusDto = badRequestObjectResult.Value as StatusDto;

            Assert.Equal(1, statusDto.Code);
        }
    }
}
