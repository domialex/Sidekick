using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    public class LeagueService : ILeagueService, IOnInit, IOnReset
    {
        private readonly IPoeApiClient poeApiClient;
        private readonly Configuration configuration;

        public LeagueService(IPoeApiClient poeApiClient,
            Configuration configuration)
        {
            this.poeApiClient = poeApiClient;
            this.configuration = configuration;
        }

        public async Task OnInit()
        {
            Leagues = null;
            Leagues = await poeApiClient.Fetch<League>();

            if (string.IsNullOrEmpty(configuration.LeagueId))
            {
                configuration.LeagueId = Leagues.FirstOrDefault().Id;
                configuration.Save();
            }
        }

        public Task OnReset()
        {
            Leagues = null;

            return Task.CompletedTask;
        }

        public List<League> Leagues { get; private set; }
    }
}
