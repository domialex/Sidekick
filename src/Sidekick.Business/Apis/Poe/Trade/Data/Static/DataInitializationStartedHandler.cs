using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly IStaticDataService staticDataService;
        private readonly ICacheRepository cacheRepository;

        public DataInitializationStartedHandler(
            IPoeTradeClient poeApiClient,
            IStaticDataService staticDataService,
            ICacheRepository cacheRepository)
        {
            this.poeApiClient = poeApiClient;
            this.staticDataService = staticDataService;
            this.cacheRepository = cacheRepository;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            var categories = await cacheRepository.GetOrSet(
                "Sidekick.Business.Apis.Poe.Trade.Data.Static.DataInitializationStartedHandler",
                () => poeApiClient.Fetch<StaticItemCategory>());

            staticDataService.ImageUrls = new Dictionary<string, string>();
            staticDataService.Ids = new Dictionary<string, string>();
            foreach (var category in categories)
            {
                foreach (var entry in category.Entries)
                {
                    staticDataService.ImageUrls.Add(entry.Id, entry.Image);
                    if (!staticDataService.Ids.ContainsKey(entry.Text))
                    {
                        staticDataService.Ids.Add(entry.Text, entry.Id);
                    }
                }
            }
        }
    }
}
