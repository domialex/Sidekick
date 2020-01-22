using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Categories
{
    public class StaticItemCategoryService : IStaticItemCategoryService, IOnInit, IOnReset
    {
        private readonly IPoeApiClient poeApiClient;

        public StaticItemCategoryService(IPoeApiClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<StaticItemCategory> Categories { get; private set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<StaticItemCategory>();
        }

        public Task OnReset()
        {
            Categories = null;

            return Task.CompletedTask;
        }
    }
}
