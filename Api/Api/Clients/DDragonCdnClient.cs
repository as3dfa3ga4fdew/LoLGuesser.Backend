using Api.Clients.Interfaces;
using Api.Models.DDragonClasses;

namespace Api.Clients
{
    public class DDragonCdnClient : IDDragonCdnClient
    {
        private readonly HttpClient _httpClient;

        public DDragonCdnClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            httpClient.BaseAddress = new Uri("https://ddragon.leagueoflegends.com/");
        }

        public async Task<Root> GetDataAsync(string version)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Root>("cdn/"+version+"/data/en_US/championFull.json");
            }
            catch(Exception e)
            {
                //log
                return null;
            }
        }

        public async Task<List<string>> GetVersionsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<string>>("api/versions.json");
            }
            catch (Exception e)
            {
                //log
                return null;
            }
        }
    }
}
