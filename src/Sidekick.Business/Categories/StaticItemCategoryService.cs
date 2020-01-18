using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Categories.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Categories
{
    public class StaticItemCategoryService : IStaticItemCategoryService, IOnBeforeInit, IOnReset
    {
        private readonly IPoeApiService poeApiService;

        public StaticItemCategoryService(IPoeApiService poeApiService)
        {
            this.poeApiService = poeApiService;
        }

        public List<StaticItemCategory> Categories { get; private set; }

        public async Task OnBeforeInit()
        {
            Categories = null;
            Categories = await poeApiService.Fetch<StaticItemCategory>("Static item categories", "static");
        }

        public Task OnReset()
        {
            Categories = null;

            return Task.CompletedTask;
        }
    }
}
