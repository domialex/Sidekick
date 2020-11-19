using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Leagues.Notifications;

namespace Sidekick.Infrastructure.PoeNinja
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
