namespace Api.Models.DDragonClasses
{
    public class Root
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public string Version { get; set; }
        public Dictionary<string, Champion> Data { get; set; }
    }
}
