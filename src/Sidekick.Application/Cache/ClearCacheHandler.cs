using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Cache.Commands;

namespace Sidekick.Application.Initialization
{
    public class ClearCacheHandler : ICommandHandler<ClearCacheCommand>
    {
        private readonly ICacheRepository cacheRepository;
        private readonly IMediator mediator;

        public ClearCacheHandler(
            ICacheRepository cacheRepository,
            IMediator mediator)
        {
            this.cacheRepository = cacheRepository;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(ClearCacheCommand request, CancellationToken cancellationToken)
        {
            await cacheRepository.Clear();
            await mediator.Publish(new CacheClearedNotification());

            return Unit.Value;
        }
    }
}
