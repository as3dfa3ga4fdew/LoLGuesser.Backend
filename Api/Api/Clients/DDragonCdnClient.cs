using Api.Clients.Interfaces;
using Api.Models.DDragonClasses;

namespace Api.Clients
{
    public class DDragonCdnClient : IDDragonCdnClient
    {
        private HttpClient _httpClient;

        public DDragonCdnClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://ddragon.leagueoflegends.com/");
        }

        public async Task<bool> TryGetDataAsync(string version, Action<Root> result)
        {
            try
            {
                Root data = await _httpClient.GetFromJsonAsync<Root>("cdn/" + version + "/data/en_US/championFull.json");
                
                result(data);

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> TryGetVersionsAsync(Action<List<string>> result)
        {
            try
            {
                List<string> data = await _httpClient.GetFromJsonAsync<List<string>>("api/versions.json");

                result(data);

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
