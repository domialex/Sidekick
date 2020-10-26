using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Leagues;

namespace Sidekick.Business.Apis.PoeNinja
{
    public class LeagueChangedHandler : INotificationHandler<LeagueChangedNotification>
    {
        private readonly IPoeNinjaCache poeNinjaCache;

        public LeagueChangedHandler(IPoeNinjaCache poeNinjaCache)
        {
            this.poeNinjaCache = poeNinjaCache;
        }

        public async Task Handle(LeagueChangedNotification notification, CancellationToken cancellationToken)
        {
            await poeNinjaCache.RefreshData();
        }
    }
}
