namespace Api.Models.Classes
{
    public class ParsedChampion
    {
        public string Name { get; set; }

        public KeyValuePair<string,string> RedactedLore { get; set; }
        public KeyValuePair<string,List<string>> SplashArtUrls { get; set; }
        public KeyValuePair<string,List<string>> SpellUrls { get; set; }
    }
}
