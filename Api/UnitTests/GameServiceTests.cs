using Api.Models.Classes;
using Api.Models.Dtos;
using Api.Models.Enums;
using Api.Models.Schemas;
using Api.Services;
using Api.Services.Interfaces;
using System.Collections.Immutable;
using System.ComponentModel;

namespace UnitTests
{
    public class GameServiceTests
    {
        [Fact]
        public void GetChampionNames_WhenSuccess_ShouldReturnListOfParsedChampions()
        {
            //Arrange
            IImmutableList<string> names = new List<string>() { "" }.ToImmutableList();
            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            iDDragonCdnServiceMock.Setup(x => x.GetChampionNames()).Returns(names);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            IImmutableList<string> result = gameService.GetChampionNames();

            //Assert
            Assert.Equal(names.Count, result.Count);
        }

        [Fact]
        public void Validate_WhenValidQuestionSchema_ShouldReturnTrue()
        {
            //Arrange
            QuestionSchema questionSchema = new QuestionSchema() 
            { 
               Type = QuestionType.Spell
            };
            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(questionSchema);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Validate_WhenInvalidQuestionSchema_ShouldReturnFalse()
        {
            //Arrange
            QuestionSchema questionSchema = new QuestionSchema()
            {
                Type = (QuestionType)100
            };
            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(questionSchema);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenValidAnswerSchema_ShouldReturnTrue()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = "name"
            };

            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(answerSchema);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Validate_WhenInvalidTypeInAnswerSchema_ShouldReturnFalse()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = (QuestionType)100,
                Answer = "name"
            };

            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(answerSchema);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenInvalidAnswerInAnswerSchema_ShouldReturnFalse()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema()
            {
                Id = "",
                Type = QuestionType.Lore,
                Answer = ""
            };

            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(answerSchema);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_WhenSchemaIsNull_ShouldReturnFalse()
        {
            //Arrange
            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act
            bool result = gameService.Validate(It.IsAny<Type>());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Validate_WhenInvalidSchemaType_ShouldThrowException()
        {
            //Arrange
            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act + Assert
            Assert.Throws<Exception>(() => gameService.Validate(new { }));
        }

        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void GetQuestion_WhenValidType_ShouldReturnQuestionDto(QuestionType type)
        {
            //Arrange
            ParsedChampion parsedChampion = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" })
            };

            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            iDDragonCdnServiceMock.Setup(x => x.GetRandomParsedChampion()).Returns(parsedChampion);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            QuestionDto result = gameService.GetQuestion(type);

            //Assert
            Assert.Equal(type, result.Type);
        }
        [Fact]
        public void GetQuestion_WhenInvalidType_ShouldThrowInvalidEnumArgumentException()
        {
            //Arrange
            ParsedChampion parsedChampion = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" })
            };

            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            iDDragonCdnServiceMock.Setup(x => x.GetRandomParsedChampion()).Returns(parsedChampion);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            Assert.Throws<InvalidEnumArgumentException>(() => gameService.GetQuestion((QuestionType)100));
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenValidTypeAndCorrectAnswer_ShouldReturnTrue(QuestionType type)
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema
            {
                Id = "",
                Type = type,
                Answer = "name"
            };

            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" })
            };

            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            bool result = gameService.VerifyAnswer(answerSchema);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyAnswer_WhenInvalidType_ShouldThrowInvalidEnumArgumentException()
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema
            {
                Id = "",
                Type = (QuestionType)100,
                Answer = "name"
            };
            GameService gameService = new GameService(It.IsAny<IDDragonCdnService>());

            //Act + Assert
            Assert.Throws<InvalidEnumArgumentException>(() => gameService.VerifyAnswer(answerSchema));
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenInvalidAnswer_ShouldReturnFalse(QuestionType type)
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema
            {
                Id = "",
                Type = type,
                Answer = "namee"
            };

            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("", new List<string>() { "" })
            };

            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            bool result = gameService.VerifyAnswer(answerSchema);

            //Assert
            Assert.False(result);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void VerifyAnswer_WhenInvalidId_ShouldThrowKeyNotFoundException(QuestionType type)
        {
            //Arrange
            AnswerSchema answerSchema = new AnswerSchema
            {
                Id = "",
                Type = type,
                Answer = "namee"
            };

            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = null;

            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act + Assert
            Assert.Throws<KeyNotFoundException>(() => gameService.VerifyAnswer(answerSchema));
        }

        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void GetParsedChampionById_WhenvalidId_ShouldReturnParsedChampion(QuestionType type)
        {
            //Arrange
            string id = "1";
            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = new ParsedChampion()
            {
                Name = "name",
                RedactedLore = new KeyValuePair<string, string>("1", ""),
                SpellUrls = new KeyValuePair<string, List<string>>("1", new List<string>() { "" }),
                SplashArtUrls = new KeyValuePair<string, List<string>>("1", new List<string>() { "" })
            };

            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            ParsedChampion result = gameService.GetParsedChampionById(type, id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(parsedChampion.Name, result.Name);
        }
        [Theory]
        [InlineData(QuestionType.Spell)]
        [InlineData(QuestionType.Splash)]
        [InlineData(QuestionType.Lore)]
        public void GetParsedChampionById_WhenInvalidId_ShouldReturnNull(QuestionType type)
        {
            //Arrange
            string id = "2";
            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();
            ParsedChampion parsedChampion = null;

            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionByLoreId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySplashId(It.IsAny<string>(), out parsedChampion)).Returns(true);
            iDDragonCdnServiceMock.Setup(x => x.GetParsedChampionBySpellId(It.IsAny<string>(), out parsedChampion)).Returns(true);

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act
            ParsedChampion result = gameService.GetParsedChampionById(type, id);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetParsedChampionById_WhenInvalidType_ShouldThrowInvalidEnumArgumentException()
        {
            // Arrange
            string id = "2";
            Mock<IDDragonCdnService> iDDragonCdnServiceMock = new Mock<IDDragonCdnService>();

            GameService gameService = new GameService(iDDragonCdnServiceMock.Object);

            //Act + Assert
            Assert.Throws<InvalidEnumArgumentException>(() => gameService.GetParsedChampionById((QuestionType)100, id));
        }
       
    }
}
