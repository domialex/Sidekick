using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Leagues.Models;
using Sidekick.Business.Loggers;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    [SidekickService(typeof(ILeagueService))]
    public class LeagueService : ILeagueService
    {
        private readonly ILogger logger;
        private readonly IPoeApiService poeApiService;

        public LeagueService(ILogger logger,
            IPoeApiService poeApiService,
            IInitializeService initializeService)
        {
            this.logger = logger;
            this.poeApiService = poeApiService;

            initializeService.OnBeforeInitialize += InitializeService_OnBeforeInitialize; ;
            initializeService.OnReset += InitializeService_OnReset;
        }

        private async Task InitializeService_OnBeforeInitialize()
        {
            logger.Log("Fetching Path of Exile league data.");

            Leagues = null;
            Leagues = await poeApiService.Fetch<League>("Leagues", "leagues");

            SelectedLeague = Leagues.FirstOrDefault();

            logger.Log($"Path of Exile league data fetched.");
        }

        private Task InitializeService_OnReset()
        {
            Leagues = null;
            SelectedLeague = null;
            return Task.CompletedTask;
        }

        public List<League> Leagues { get; private set; }

        public League SelectedLeague { get; set; }
    }
}
