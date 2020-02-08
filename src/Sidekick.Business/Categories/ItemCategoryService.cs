using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Categories
{
    public class ItemCategoryService : IItemCategoryService, IOnInit, IDisposable
    {
        private readonly IPoeApiClient poeApiClient;

        public ItemCategoryService(IPoeApiClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<ItemCategory> Categories { get; private set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<ItemCategory>();
        }

        public void Dispose()
        {
            Categories = null;
        }
    }
}
