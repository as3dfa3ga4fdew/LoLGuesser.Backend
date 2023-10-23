using Api.Services.Interfaces;

namespace Api.BackgroundServices
{
    public class DDragonCdnBackgroundService : BackgroundService
    {
        private readonly IDDragonCdnService _dDragonCdnService;

        public DDragonCdnBackgroundService(IDDragonCdnService dDragonCdnService)
        {
            _dDragonCdnService = dDragonCdnService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
