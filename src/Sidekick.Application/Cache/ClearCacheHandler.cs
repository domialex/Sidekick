using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Apis.PoeNinja;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Cache.Commands;

namespace Sidekick.Application.Initialization
{
    public class ClearCacheHandler : ICommandHandler<ClearCacheCommand>
    {
        private readonly ICacheRepository cacheRepository;
        private readonly IPoeNinjaRepository poeNinjaRepository;

        public ClearCacheHandler(
            ICacheRepository cacheRepository,
            IPoeNinjaRepository poeNinjaRepository)
        {
            this.cacheRepository = cacheRepository;
            this.poeNinjaRepository = poeNinjaRepository;
        }

        public async Task<Unit> Handle(ClearCacheCommand request, CancellationToken cancellationToken)
        {
            await cacheRepository.Clear();
            await poeNinjaRepository.Clear();

            return Unit.Value;
        }
    }
}
