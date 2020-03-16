using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Apis.Poe.Trade.Leagues
{
    public class LeagueDataService : ILeagueDataService, IOnInit
    {
        private readonly IPoeApiClient poeApiClient;
        private readonly SidekickSettings settings;

        public List<League> Leagues { get; private set; }

        public LeagueDataService(IPoeApiClient poeApiClient,
            SidekickSettings settings)
        {
            this.poeApiClient = poeApiClient;
            this.settings = settings;
        }

        public event Action OnNewLeagues;

        public async Task OnInit()
        {
            Leagues = null;
            Leagues = await poeApiClient.Fetch<League>();

            var newLeagues = false;

            using var algorithm = SHA256.Create();
            var leaguesHash = Encoding.UTF8.GetString(
                algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(Leagues)
                    )
                )
            );

            if (leaguesHash != settings.LeaguesHash)
            {
                settings.LeaguesHash = leaguesHash;
                settings.Save();
                newLeagues = true;
            }

            if (string.IsNullOrEmpty(settings.LeagueId) || !Leagues.Any(x => x.Id == settings.LeagueId))
            {
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
