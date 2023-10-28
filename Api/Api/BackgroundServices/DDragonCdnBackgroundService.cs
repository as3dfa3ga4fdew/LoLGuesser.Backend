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
            int shortDelay = 3600000; //1 hour in ms

            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> versions = null;
                if(!await _dDragonCdnClient.TryGetVersionsAsync(x => versions = x))
                {
                    _logger.LogWarning("Could not get versions. Delaying for " + shortDelay + "ms.");
                    await Task.Delay(shortDelay);
                    continue;
                }

                Root root = null;
                if(!await _dDragonCdnClient.TryGetDataAsync(versions[0], x => root = x))
                {
                    _logger.LogWarning("Could not get root. Delaying for " + shortDelay + "ms.");
                    await Task.Delay(shortDelay);
                    continue;
                }

                if (!root.TryConvertToImmutableParsedChampionList(out var parsedChampionList))
                {
                    _logger.LogWarning("Could not convert root to parsed champion list. Delaying for " + shortDelay + "ms.");
                    await Task.Delay(shortDelay);
                    continue;
                }

                try
                {
                    _dDragonCdnService.UpdateParsedChampions(parsedChampionList);
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Could not update DDragonCdnService. Delaying for " + shortDelay + "ms.");
                    await Task.Delay(shortDelay);
                    continue;
                }
               

                _logger.LogInformation("Succesfully received, parsed and updated data. Delaying for " + delay + "ms.");
                await Task.Delay(delay);
            }
        }
    }
}
