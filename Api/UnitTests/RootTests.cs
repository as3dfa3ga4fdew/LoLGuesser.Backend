using Api.Models.DDragonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class RootTests
    {
        [Fact]
        public void TryConvertToImmutableParsedChampionList_WhenPropertiesAreFilled_ShouldReturnTrueAndImmutableParsedChampionList()
        {
            //Arrange
            Root root = new Root()
            {
                Type = "champion",
                Format = "full",
                Version = "13.20.1",
                Data = new Dictionary<string, Champion>
                {
                    {
                        "Aatrox",
                        new Champion
                        {
                            Name = "Aatrox",
                            Skins = new List<Skin>
                            {
                                new Skin { Num = 0 },
                                new Skin { Num = 1 },
                                new Skin { Num = 2 },
                                new Skin { Num = 3 },
                                new Skin { Num = 7 },
                                new Skin { Num = 8 },
                                new Skin { Num = 9 },
                                new Skin { Num = 11 },
                                new Skin { Num = 20 },
                                new Skin { Num = 21 },
                                new Skin { Num = 30 },
                                new Skin { Num = 31 },
                            },
                            Spells = new List<Spell>
                            {
                                new Spell { Id = "AatroxQ" },
                                new Spell { Id = "AatroxW" },
                                new Spell { Id = "AatroxE" },
                                new Spell { Id = "AatroxR" },
                            },
                            Lore = "Once honored defenders of Shurima against the Void, Aatrox and his brethren would eventually become an even greater threat to Runeterra, and were defeated only by cunning mortal sorcery. But after centuries of imprisonment, Aatrox was the first to find freedom once more, corrupting and transforming those foolish enough to try and wield the magical weapon that contained his essence. Now, with stolen flesh, he walks Runeterra in a brutal approximation of his previous form, seeking an apocalyptic and long overdue vengeance."
                        }
                    }
                }
            };

            //Act
            bool isSuccess = root.TryConvertToImmutableParsedChampionList(out var championList);

            //Assert
            Assert.True(isSuccess);
            Assert.NotNull(championList);
            Assert.Equal(root.Data.Values.Count, championList.Count);
        }

        [Fact]
        public void TryConvertToImmutableParsedChampionList_WhenAnyExceptionThrown_ShouldReturnFalseAndNull()
        {
            //Arrange
            Root root = new Root() { };

            //Act
            bool isSuccess = root.TryConvertToImmutableParsedChampionList(out var championList);

            //Assert
            Assert.False(isSuccess);
            Assert.Null(championList);
        }
    }
}
