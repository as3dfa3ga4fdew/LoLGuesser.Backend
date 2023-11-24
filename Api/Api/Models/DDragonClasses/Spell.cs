namespace Api.Models.DDragonClasses
{
    public class Spell
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public LevelTip LevelTip { get; set; }
        public int MaxRank { get; set; }
        public List<double> Cooldown { get; set; }
        public string CooldownBurn { get; set; }
        public List<int> Cost { get; set; }
        public string CostBurn { get; set; }
        public List<object> Effect { get; set; }
        public List<object> Vars { get; set; }
        public string CostType { get; set; }
        public string MaxAmmo { get; set; }
        public List<double> Range { get; set; }
        public string RangeBurn { get; set; }
        public Image Image { get; set; }
        public string Resource { get; set; }
    }
}
