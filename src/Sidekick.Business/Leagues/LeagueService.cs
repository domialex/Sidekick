using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Leagues.Models;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    [SidekickService(typeof(ILeagueService))]
    public class LeagueService : ILeagueService, IOnBeforeInitialize, IOnReset
    {
        private readonly ILogger logger;
        private readonly IPoeApiService poeApiService;

        public LeagueService(ILogger logger,
            IPoeApiService poeApiService)
        {
            this.logger = logger;
            this.poeApiService = poeApiService;
        }

        public async Task OnBeforeInitialize()
        {
            logger.Log("Fetching Path of Exile league data.");

            Leagues = null;
            Leagues = await poeApiService.Fetch<League>("Leagues", "leagues");

            SelectedLeague = Leagues.FirstOrDefault();

            logger.Log($"Path of Exile league data fetched.");
        }

        public Task OnReset()
        {
            Leagues = null;
            SelectedLeague = null;
            return Task.CompletedTask;
        }

        public List<League> Leagues { get; private set; }

        public League SelectedLeague { get; set; }
    }
}
