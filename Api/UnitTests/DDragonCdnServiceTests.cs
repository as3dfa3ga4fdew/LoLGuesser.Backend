using Api.Models.Classes;
using Api.Models.Enums;
using Api.Services;
using Api.Services.Interfaces;
using Moq;
using System;
using System.Collections.Immutable;

namespace UnitTests
{
    public class DDragonCdnServiceTests
    {
        [Fact]
        public void GetChampionNames_WhenParsedChampionsPropertyIsAssigned_ShouldReturnIImmutableList()
        {
            //Arrange
            IImmutableList<ParsedChampion> parsedChampions = new List<ParsedChampion>() { new ParsedChampion() { Name = "champion1" }, new ParsedChampion() { Name = "champion2" } }.ToImmutableList();
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            DDragonCdnService dDragonCdnService = (DDragonCdnService)iDDragonCdnService;
            dDragonCdnService.ParsedChampions = parsedChampions;

            //Act
            IImmutableList<string> champions = iDDragonCdnService.GetChampionNames();

            //Assert
            Assert.NotNull(champions);
            Assert.Equal(2, champions.Count);
        }
        [Fact]
        public void GetChampionNames_WhenParsedChampionsPropertyIsNotAssinged_ShouldThrowInvalidOperationException()
        {
            //Arrange
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(iDDragonCdnService.GetChampionNames);
        }
        [Fact]
        public void GetRandomParsedChampion_WhenParsedChampionsPropertyIsAssigned_ShouldReturnParsedChampion()
        {
            //Arrange
            IImmutableList<ParsedChampion> parsedChampions = new List<ParsedChampion>() { new ParsedChampion() { Name = "champion1" }, new ParsedChampion() { Name = "champion2" } }.ToImmutableList();
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            DDragonCdnService dDragonCdnService = (DDragonCdnService)iDDragonCdnService;
            dDragonCdnService.ParsedChampions = parsedChampions;

            //Act
            ParsedChampion parsedChampion = iDDragonCdnService.GetRandomParsedChampion();

            //Assert
            Assert.NotNull(parsedChampion);
            Assert.Contains(parsedChampion, parsedChampions);
        }

        [Fact]
        public void GetRandomParsedChampion_WhenParsedChampionsPropertyIsNotAssinged_ShouldThrowInvalidOperationException()
        {
            //Arrange
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(iDDragonCdnService.GetRandomParsedChampion);
        }

        [Fact]
        public void UpdateParsedChampions_WhenGivenImmutableParsedChampionListWithItems_ShouldSetProperty()
        {
            //Arrange
            IImmutableList<ParsedChampion> parsedChampions = new List<ParsedChampion>() { new ParsedChampion() { Name = "champion1" }, new ParsedChampion() { Name = "champion2" } }.ToImmutableList();
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act
            iDDragonCdnService.UpdateParsedChampions(parsedChampions);

            //Assert
            DDragonCdnService dDragonCdnService = (DDragonCdnService)iDDragonCdnService;
            Assert.NotNull(dDragonCdnService.ParsedChampions);
            Assert.Equal(dDragonCdnService.ParsedChampions, parsedChampions);
        }

        [Fact]
        public void UpdateParsedChampions_WhenGivenNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            IImmutableList<ParsedChampion> parsedChampions = null;
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<ArgumentNullException>(() => iDDragonCdnService.UpdateParsedChampions(parsedChampions));
        }

        [Fact]
        public void UpdateParsedChampions_WhenGivenEmptyImmutableParsedChampionList_ShouldThrowArgumentOutOfRangeException()
        {
            //Arrange
            IImmutableList<ParsedChampion> parsedChampions = new List<ParsedChampion>().ToImmutableList();
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act
            Assert.Throws<ArgumentOutOfRangeException>(() => iDDragonCdnService.UpdateParsedChampions(parsedChampions));
        }

        [Fact]
        public void TryGetParsedChampionBySpellId_WhenIdExists_ShouldReturnTrueAndParsedChampion()
        {
            //Arrange
            string id = "1";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            { 
                new ParsedChampion() { Name = "", SpellUrls = new KeyValuePair<string, List<string>>("1", new List<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionBySpellId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.True(isSucess);
            Assert.Equal(id, parsedChampion.SpellUrls.Key);
        }
        [Fact]
        public void TryGetParsedChampionBySpellId_WhenIdDoesNotExist_ShouldReturnFalseAndNull()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            {
                new ParsedChampion() { Name = "", SpellUrls = new KeyValuePair<string, List<string>>("1", new List<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionBySpellId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.False(isSucess);
            Assert.Null(parsedChampion);
        }
        [Fact]
        public void TryGetParsedChampionBySpellId_WhenParsedChampionsPropertyIsNotAssinged_ShouldThrowInvalidOperationException()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => iDDragonCdnService.GetParsedChampionBySpellId(id, out _));
        }
        [Fact]
        public void TryGetParsedChampionByLoreId_WhenIdExists_ShouldReturnTrueAndParsedChampion()
        {
            //Arrange
            string id = "1";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            {
                new ParsedChampion() { Name = "", RedactedLore = new KeyValuePair<string, string>("1", It.IsAny<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionByLoreId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.True(isSucess);
            Assert.Equal(id, parsedChampion.RedactedLore.Key);
        }
        [Fact]
        public void TryGetParsedChampionByLoreId_WhenIdDoesNotExist_ShouldReturnFalseAndNull()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            {
                new ParsedChampion() { Name = "", RedactedLore = new KeyValuePair<string, string>("1", It.IsAny<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionByLoreId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.False(isSucess);
            Assert.Null(parsedChampion);
        }
        [Fact]
        public void TryGetParsedChampionByLoreId_WhenParsedChampionsPropertyIsNotAssinged_ShouldThrowInvalidOperationException()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => iDDragonCdnService.GetParsedChampionByLoreId(id, out _));
        }
        [Fact]
        public void TryGetParsedChampionBySplashId_WhenIdExists_ShouldReturnTrueAndParsedChampion()
        {
            //Arrange
            string id = "1";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            {
                new ParsedChampion() { Name = "", SplashArtUrls = new KeyValuePair<string, List<string>>("1", new List<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionBySplashId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.True(isSucess);
            Assert.Equal(id, parsedChampion.SplashArtUrls.Key);
        }
        [Fact]
        public void TryGetParsedChampionBySplashId_WhenIdDoesNotExist_ShouldReturnFalseAndNull()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();
            ((DDragonCdnService)iDDragonCdnService).ParsedChampions = new List<ParsedChampion>()
            {
                new ParsedChampion() { Name = "", SplashArtUrls = new KeyValuePair<string, List<string>>("1", new List<string>()) }
            }.ToImmutableList();

            //Act
            bool isSucess = iDDragonCdnService.GetParsedChampionBySplashId(id, out ParsedChampion parsedChampion);

            //Assert
            Assert.False(isSucess);
            Assert.Null(parsedChampion);
        }
        [Fact]
        public void TryGetParsedChampionBySplashId_WhenParsedChampionsPropertyIsNotAssinged_ShouldThrowInvalidOperationException()
        {
            //Arrange
            string id = "0";
            IDDragonCdnService iDDragonCdnService = new DDragonCdnService();

            //Act + Assert
            Assert.Throws<InvalidOperationException>(() => iDDragonCdnService.GetParsedChampionBySplashId(id, out _));
        }
    }
}
