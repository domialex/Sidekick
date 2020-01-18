using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Categories.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Categories
{
    public class ItemCategoryService : IItemCategoryService, IOnBeforeInit, IOnReset
    {
        private readonly IPoeApiService poeApiService;

        public ItemCategoryService(IPoeApiService poeApiService)
        {
            this.poeApiService = poeApiService;
        }

        public List<ItemCategory> Categories { get; private set; }

        public async Task OnBeforeInit()
        {
            Categories = null;
            Categories = await poeApiService.Fetch<ItemCategory>("Item categories", "items");
        }

        public Task OnReset()
        {
            Categories = null;

            return Task.CompletedTask;
        }
    }
}
