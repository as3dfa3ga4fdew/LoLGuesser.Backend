namespace Api.Models.Classes
{
    public class ParsedChampion
    {
        public string Name { get; set; }
        public string RedactedLore { get; set; }
        public List<string> SplashArtUrls { get; set; }
        public List<string> SpellUrls { get; set; }
    }
}
