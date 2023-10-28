using Api.Models.Classes;
using Api.Services;
using Api.Services.Interfaces;
using System;
using System.Collections.Immutable;

namespace UnitTests
{
    public class DDragonCdnServiceTests
    {
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
    }
}
