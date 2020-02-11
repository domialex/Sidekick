using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Leagues
{
    public class LeagueService : ILeagueService, IOnInit, IDisposable
    {
        private readonly IPoeApiClient poeApiClient;
        private readonly SidekickSettings configuration;

        public List<League> Leagues { get; private set; }

        public LeagueService(IPoeApiClient poeApiClient,
            SidekickSettings configuration)
        {
            this.poeApiClient = poeApiClient;
            this.configuration = configuration;
        }

        public async Task OnInit()
        {
            Leagues = null;
            Leagues = await poeApiClient.Fetch<League>();
            if (string.IsNullOrEmpty(configuration.LeagueId) ||
                !Leagues.Exists(x => x.Id == configuration.LeagueId))
            {
                configuration.LeagueId = Leagues.FirstOrDefault().Id;
                configuration.Save();
            }
        }

        public void Dispose()
        {
            Leagues = null;
        }
    }
}
