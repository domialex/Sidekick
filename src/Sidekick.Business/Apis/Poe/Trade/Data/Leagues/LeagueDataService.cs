using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sidekick.Business.Caches;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Apis.Poe.Trade.Leagues
{
    public class LeagueDataService : ILeagueDataService, IOnBeforeInit
    {
        private readonly IPoeTradeClient poeTradeClient;
        private readonly SidekickSettings settings;
        private readonly ICacheService cacheService;

        public List<League> Leagues { get; private set; }

        public LeagueDataService(IPoeTradeClient poeTradeClient,
            SidekickSettings settings,
            ICacheService cacheService)
        {
            this.poeTradeClient = poeTradeClient;
            this.settings = settings;
            this.cacheService = cacheService;
        }

        public event Action OnLeagueChange;
        public event Action OnNewLeagues;

        public void LeagueChanged() => OnLeagueChange.Invoke();

        public async Task OnBeforeInit()
        {
            Leagues = null;
            Leagues = await poeTradeClient.Fetch<League>();

            var newLeagues = false;

            using var algorithm = SHA256.Create();
            var leaguesHash = Encoding.UTF8.GetString(
                algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(Leagues.Select(x => x.Id).ToList())
                    )
                )
            );

            if (leaguesHash != settings.LeaguesHash)
            {
                await cacheService.Clear();
                settings.LeaguesHash = leaguesHash;
                settings.Save();
                newLeagues = true;
            }

            if (string.IsNullOrEmpty(settings.LeagueId) || !Leagues.Any(x => x.Id == settings.LeagueId))
            {
                await cacheService.Clear();
                settings.LeagueId = Leagues.FirstOrDefault().Id;
                settings.Save();
                newLeagues = true;
            }

            if (newLeagues && OnNewLeagues != null)
            {
                OnNewLeagues.Invoke();
            }
        }
    }
}
