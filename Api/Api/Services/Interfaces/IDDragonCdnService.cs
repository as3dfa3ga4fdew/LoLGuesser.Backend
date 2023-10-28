﻿using Api.Models.Classes;
using Api.Models.DDragonClasses;
using System.Collections.Immutable;

namespace Api.Services.Interfaces
{
    public interface IDDragonCdnService
    {
        public ParsedChampion GetRandomParsedChampion();
        public void UpdateParsedChampions(IImmutableList<ParsedChampion> parsedChampions);
    }
}
