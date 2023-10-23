using Api.Clients.Interfaces;

namespace Api.Clients
{
    public class DDragonCdnClient : IDDragonCdnClient
    {
        private readonly HttpClient _httpClient;

        public DDragonCdnClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
