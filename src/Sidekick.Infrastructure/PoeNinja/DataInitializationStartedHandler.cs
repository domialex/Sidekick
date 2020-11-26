using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Infrastructure.PoeNinja
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IPoeNinjaCache poeNinjaCache;

        public DataInitializationStartedHandler(
            IPoeNinjaCache poeNinjaCache)
        {
            this.poeNinjaCache = poeNinjaCache;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            await poeNinjaCache.RefreshData();
        }
    }
}
