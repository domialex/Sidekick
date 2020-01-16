using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Languages;
using Sidekick.Business.Leagues.Models;
using Sidekick.Business.Loggers;
using Sidekick.Core.DependencyInjection.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    [SidekickService(typeof(ILeagueService))]
    public class LeagueService : ILeagueService
    {
        private readonly ILogger logger;
        private readonly IPoeApiService poeApiService;

        public LeagueService(ILogger logger,
            ILanguageProvider languageProvider,
            IPoeApiService poeApiService)
        {
            this.logger = logger;
            this.poeApiService = poeApiService;

            languageProvider.LanguageChanged += Initialize;
        }

        public List<League> Leagues { get; private set; }

        public League SelectedLeague { get; set; }

        public async Task Initialize()
        {
            logger.Log("Fetching Path of Exile league data.");

            Leagues = null;
            Leagues = await poeApiService.Fetch<League>("Leagues", "leagues");

            logger.Log($"Path of Exile league data fetched.");
        }

    }
}
