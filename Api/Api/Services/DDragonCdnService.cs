using Api.Clients.Interfaces;
using Api.Models.Classes;
using Api.Models.DDragonClasses;
using Api.Services.Interfaces;
using System.Collections.Immutable;

namespace Api.Services
{
    public class DDragonCdnService : IDDragonCdnService
    {
        public IImmutableList<ParsedChampion>? ParsedChampions { get; set; }

        public DDragonCdnService() { }
       
        public void UpdateParsedChampions(IImmutableList<ParsedChampion> parsedChampions)
        {
            if(parsedChampions == null)
                throw new ArgumentNullException(nameof(parsedChampions));

            if (parsedChampions.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(parsedChampions));

            ParsedChampions = parsedChampions;
        }

        public ParsedChampion GetRandomParsedChampion()
        {
            if (ParsedChampions == null) throw new InvalidOperationException(nameof(UpdateParsedChampions));

            int pos = Random.Shared.Next(0, ParsedChampions.Count);

            return ParsedChampions[pos];
        }

        public IImmutableList<string> GetChampionNames()
        {
            if (ParsedChampions == null) throw new InvalidOperationException(nameof(UpdateParsedChampions));

            return ParsedChampions.Select(x => x.Name).ToImmutableList();
        }
    }
}
