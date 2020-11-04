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

        public ClearCacheHandler(
            ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public async Task<Unit> Handle(ClearCacheCommand request, CancellationToken cancellationToken)
        {
            await cacheRepository.Clear();

            return Unit.Value;
        }
    }
}
