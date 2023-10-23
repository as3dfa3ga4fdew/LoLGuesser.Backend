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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(86400000); //Get and process data once a day.
            }
        }
    }
}
