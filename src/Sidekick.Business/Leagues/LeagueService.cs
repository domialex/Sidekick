using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    public class LeagueService : ILeagueService, IOnInit, IOnReset
    {
        private readonly IPoeApiService poeApiService;
        private readonly Configuration configuration;

        public List<League> Leagues { get; private set; }

        public event Action LeaguesUpdated;

        public LeagueService(IPoeApiService poeApiService,
            Configuration configuration)
        {
            this.poeApiService = poeApiService;
            this.configuration = configuration;
        }

        private void NotifyLeaguesUpdated()
        {
            foreach (var handler in LeaguesUpdated?.GetInvocationList())
            {
                ((Action)handler)();
            }
        }

        public async Task OnInit()
        {
            Leagues = null;
            Leagues = await poeApiService.Fetch<League>();
            NotifyLeaguesUpdated();

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
    }
}
