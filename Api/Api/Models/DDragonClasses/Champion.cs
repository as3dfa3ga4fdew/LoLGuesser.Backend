namespace Api.Models.DDragonClasses
{
    public class Champion
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Image Image { get; set; }
        public List<Skin> Skins { get; set; }
        public string Lore { get; set; }
        public string Blurb { get; set; }
        public List<string> Allytips { get; set; }
        public List<string> Enemytips { get; set; }
        public List<string> Tags { get; set; }
        public string Partype { get; set; }
        public Info Info { get; set; }
        public Stats Stats { get; set; }
        public List<Spell> Spells { get; set; }
        public Passive Passive { get; set; }
        public List<object> Recommended { get; set; }
    }
}
