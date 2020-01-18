using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Leagues.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Leagues
{
    public class LeagueService : ILeagueService, IOnBeforeInit, IOnReset
    {
        private readonly IPoeApiService poeApiService;

        public LeagueService(IPoeApiService poeApiService)
        {
            this.poeApiService = poeApiService;
        }

        public async Task OnBeforeInit()
        {
            Leagues = null;
            Leagues = await poeApiService.Fetch<League>("Leagues", "leagues");

            SelectedLeague = Leagues.FirstOrDefault();
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
