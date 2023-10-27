using Api.Models.Classes;
using System;
using System.Collections.Immutable;

namespace Api.Models.DDragonClasses
{
    public class Root
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string Version { get; set; }
        public Dictionary<string, Champion> Data { get; set; }
    }

    public static class RootExtensions
    {
        public static IImmutableList<ParsedChampion> ToImmutableParsedChampionList(this Root root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (root.Data == null) throw new NullReferenceException(nameof(root.Data));

            return root.Data.Values.Select(x => new ParsedChampion() 
            { 
                Name = x.Name,
                RedactedLore = x.Lore.Replace(x.Name, "secret"),
                SplashArtUrls = x.Skins.Select(x => "https://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + x.Name + "_" + x.Num + ".jpg").ToList(),
                SpellUrls = x.Spells.Select(x => "https://ddragon.leagueoflegends.com/cdn/" + root.Version + "/img/spell/" + x.Id + ".png").ToList()
            }).ToImmutableList();
        }
    }
}
