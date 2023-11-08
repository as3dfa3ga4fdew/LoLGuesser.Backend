using Api.Models.Classes;
using Api.Models.DDragonClasses;
using System.Collections.Immutable;

namespace Api.Services.Interfaces
{
    public interface IDDragonCdnService
    {
        IImmutableList<string> GetChampionNames();
        bool GetParsedChampionByLoreId(string id, out ParsedChampion parsedChampion);
        bool GetParsedChampionBySpellId(string id, out ParsedChampion parsedChampion);
        bool GetParsedChampionBySplashId(string id, out ParsedChampion parsedChampion);
        public ParsedChampion GetRandomParsedChampion();
        public void UpdateParsedChampions(IImmutableList<ParsedChampion> parsedChampions);
    }
}
