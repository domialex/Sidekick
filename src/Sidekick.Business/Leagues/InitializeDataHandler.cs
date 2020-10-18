using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Trade.Data.Leagues;
using Sidekick.Business.Caches;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Leagues
{
    public class InitializeDataHandler : INotificationHandler<InitializeData>
    {
        private readonly SidekickSettings settings;
        private readonly ICacheService cacheService;
        private readonly IMediator mediator;
        private readonly ILeagueDataService leagueDataService;

        public InitializeDataHandler(
            SidekickSettings settings,
            ICacheService cacheService,
            IMediator mediator,
            ILeagueDataService leagueDataService)
        {
            this.settings = settings;
            this.cacheService = cacheService;
            this.mediator = mediator;
            this.leagueDataService = leagueDataService;
        }

        public async Task Handle(InitializeData notification, CancellationToken cancellationToken)
        {
            var leagues = await mediator.Send(new GetLeaguesQuery());
            leagueDataService.Initialize(leagues);

            var newLeagues = false;

            using var algorithm = SHA256.Create();
            var leaguesHash = Encoding.UTF8.GetString(
                algorithm.ComputeHash(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(leagues.Select(x => x.Id).ToList())
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

            if (string.IsNullOrEmpty(settings.LeagueId) || !leagues.Any(x => x.Id == settings.LeagueId))
            {
                await cacheService.Clear();
                settings.LeagueId = leagues.FirstOrDefault().Id;
                settings.Save();
                newLeagues = true;
            }

            if (newLeagues)
            {
                await mediator.Publish(new NewLeagues());
            }
        }
    }
}
