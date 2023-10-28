using Api.Models.DDragonClasses;

namespace Api.Clients.Interfaces
{
    public interface IDDragonCdnClient
    {
        public Task<bool> TryGetVersionsAsync(Action<List<string>> result);
        public Task<bool> TryGetDataAsync(string version, Action<Root> result);
    }
}
