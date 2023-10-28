using Api.Clients.Interfaces;
using Api.Models.Classes;
using Api.Models.DDragonClasses;
using Api.Services.Interfaces;
using System.Collections.Immutable;

namespace Api.BackgroundServices
{
    public class DDragonCdnBackgroundService : BackgroundService
    {
        private readonly IDDragonCdnService _dDragonCdnService;
        private readonly IDDragonCdnClient _dDragonCdnClient;
        private readonly ILogger<DDragonCdnBackgroundService> _logger;
        public DDragonCdnBackgroundService(IDDragonCdnService dDragonCdnService, IDDragonCdnClient dDragonCdnClient, ILogger<DDragonCdnBackgroundService> logger)
        {
            _dDragonCdnService = dDragonCdnService;
            _dDragonCdnClient = dDragonCdnClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = 86400000; //1 day in ms
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Data parsed, received and updated. Delaying for "+ delay +"ms.");
                    await Task.Delay(86400000, stoppingToken); //Get and process data once a day.
                }
                catch (Exception e)
                {
                    _logger.LogError(nameof(e) + " " + e.Message);
                }
            }
        }
    }
}
