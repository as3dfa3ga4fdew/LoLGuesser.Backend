using Api.Models.DDragonClasses;

namespace Api.Clients.Interfaces
{
    public interface IDDragonCdnClient
    {
        public Task<Root> GetDataAsync(string version);
        public Task<List<string>> GetVersionsAsync();
    }
}
