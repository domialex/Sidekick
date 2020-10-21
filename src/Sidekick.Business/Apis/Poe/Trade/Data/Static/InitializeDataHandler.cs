using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Caches;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public class InitializeDataHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly ICacheService cacheService;
        private readonly IStaticDataService staticDataService;

        public InitializeDataHandler(
            IPoeTradeClient poeApiClient,
            ICacheService cacheService,
            IStaticDataService staticDataService)
        {
            this.poeApiClient = poeApiClient;
            this.cacheService = cacheService;
            this.staticDataService = staticDataService;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            var categories = await cacheService.GetOrCreate("StaticDataService.OnInit", () => poeApiClient.Fetch<StaticItemCategory>());

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
