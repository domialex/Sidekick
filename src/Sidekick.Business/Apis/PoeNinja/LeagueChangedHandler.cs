using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Leagues;

namespace Sidekick.Business.Apis.PoeNinja
{
    public class LeagueChangedHandler : INotificationHandler<LeagueChanged>
    {
        private readonly IPoeNinjaCache poeNinjaCache;

        public LeagueChangedHandler(IPoeNinjaCache poeNinjaCache)
        {
            this.poeNinjaCache = poeNinjaCache;
        }

        public async Task Handle(LeagueChanged notification, CancellationToken cancellationToken)
        {
            await poeNinjaCache.RefreshData();
        }
    }
}
