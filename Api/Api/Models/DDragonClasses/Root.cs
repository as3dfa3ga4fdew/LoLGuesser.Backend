using Api.Helpers;
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

        public bool TryConvertToImmutableParsedChampionList(out IImmutableList<ParsedChampion> result)
        {
            try
            {
                Md5 md5 = new Md5();
                List<ParsedChampion> parsedChampions = new List<ParsedChampion>();
                foreach(var champion in Data.Values)
                {
                    ParsedChampion parsedChampion = new ParsedChampion();

                    parsedChampion.Name = champion.Name;

                    string redactedLore = champion.Lore.Replace(champion.Name, champion.Lore.Replace(champion.Name, "secret"));
                    parsedChampion.RedactedLore = new KeyValuePair<string, string>(md5.Hash("Lore", champion.Name), redactedLore);

                    List<string> splashArtUrls = champion.Skins.Select(x => "https://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + champion.Id + "_" + x.Num + ".jpg").ToList();
                    parsedChampion.SplashArtUrls = new KeyValuePair<string, List<string>>(md5.Hash("Splash", champion.Name), splashArtUrls);

                    List<string> spellUrls = champion.Spells.Select(x => "https://ddragon.leagueoflegends.com/cdn/" + Version + "/img/spell/" + x.Id + ".png").ToList();
                    parsedChampion.SpellUrls = new KeyValuePair<string, List<string>>(md5.Hash("Spell", champion.Name), spellUrls);

                    parsedChampions.Add(parsedChampion);
                }

                result = parsedChampions.ToImmutableList();
                
                return true;
            }
            catch (Exception e)
            {
                result = null;
                return false;
            }
        }

    }
}
