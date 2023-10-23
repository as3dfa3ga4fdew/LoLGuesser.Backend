using Api.Clients.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class DDragonCdnService : IDDragonCdnService
    {
        private readonly IDDragonCdnClient _dDragonCdnClient;

        public DDragonCdnService(IDDragonCdnClient dDragonCdnClient)
        {
            _dDragonCdnClient = dDragonCdnClient;
        }
    }
}
