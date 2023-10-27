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

        public DDragonCdnBackgroundService(IDDragonCdnService dDragonCdnService, IDDragonCdnClient dDragonCdnClient)
        {
            _dDragonCdnService = dDragonCdnService;
            _dDragonCdnClient = dDragonCdnClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //Get version && wait if failed
                    List<string> version = await _dDragonCdnClient.GetVersionsAsync();
                    if (version == null)
                    {
                        await Task.Delay(1000000);
                        continue;
                    }

                    //Get root && wait if failed
                    Root root = await _dDragonCdnClient.GetDataAsync(version[0]);
                    if (root == null)
                    {
                        await Task.Delay(1000000);
                        continue;
                    }

                    //Parse
                    IImmutableList<ParsedChampion> parsedChampions = root.ToImmutableParsedChampionList();

                    //Set root on DDragonCdnService
                    _dDragonCdnService.UpdateParsedChampions(parsedChampions);

                    await Task.Delay(86400000); //Get and process data once a day.
                }
                catch (Exception e)
                {
                    //Log
                }
            }
        }
    }
}
