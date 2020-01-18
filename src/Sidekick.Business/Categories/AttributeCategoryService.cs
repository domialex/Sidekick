using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Categories.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Categories
{
    public class AttributeCategoryService : IAttributeCategoryService, IOnBeforeInit, IOnReset
    {
        private readonly IPoeApiService poeApiService;

        public AttributeCategoryService(IPoeApiService poeApiService)
        {
            this.poeApiService = poeApiService;
        }

        public List<AttributeCategory> Categories { get; private set; }

        public async Task OnBeforeInit()
        {
            Categories = null;
            Categories = await poeApiService.Fetch<AttributeCategory>("Attribute categories", "stats");
        }

        public Task OnReset()
        {
            Categories = null;

            return Task.CompletedTask;
        }
    }
}
