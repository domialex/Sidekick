using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Apis.PoeNinja;
using Sidekick.Domain.Cache.Commands;

namespace Sidekick.Infrastructure.PoeNinja
{
    public class CacheClearedHandler : INotificationHandler<CacheClearedNotification>
    {
        private readonly IPoeNinjaRepository repository;

        public CacheClearedHandler(IPoeNinjaRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(CacheClearedNotification notification, CancellationToken cancellationToken)
        {
            await repository.Clear();
        }
    }
}
