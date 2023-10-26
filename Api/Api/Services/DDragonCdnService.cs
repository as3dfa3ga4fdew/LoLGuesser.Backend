using Api.Clients.Interfaces;
using Api.Models.Classes;
using Api.Models.DDragonClasses;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class DDragonCdnService : IDDragonCdnService
    {
        private readonly IDDragonCdnClient _dDragonCdnClient;
        private Root? _root;

        public DDragonCdnService(IDDragonCdnClient dDragonCdnClient)
        {
            _dDragonCdnClient = dDragonCdnClient;
        }
        public async Task UpdateAsync()
        {
            List<string> versions = await _dDragonCdnClient.GetVersionsAsync();
            if (versions == null)
                return;

            Root root = await _dDragonCdnClient.GetDataAsync(versions[0]);
            if(root == null) 
                return;

            _root = root;
        }

        public void SetRoot(Root root)
        {
            if (root == null)
                throw new ArgumentNullException();

            _root = root;
        }

        public Champion GetRandomChampion()
        {
            if (_root.Data == null)
                return null;

            int pos = Random.Shared.Next(0, _root.Data.Count);

            Champion champion = _root.Data.Values.ToArray()[pos];

            return champion;
        }

        public ParsedChampion ParseChampion(Champion champion, string version)
        {
           return new ParsedChampion()
            {
                Name = champion.Name,
                RedactedLore = champion.Lore.Replace(champion.Name, "secret"),
                SplashArtUrls = champion.Skins.Select(x => "https://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + champion.Name + "_" + x.Num + ".jpg").ToList(),
                SpellUrls = champion.Spells.Select(x => "https://ddragon.leagueoflegends.com/cdn/"+version+"/img/spell/" + x.Id + ".png").ToList()
            };
        }
    }
}
