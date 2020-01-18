using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Categories.Models;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Business.Categories
{
    public class StaticItemCategoryService : IStaticItemCategoryService, IOnBeforeInit, IOnReset
    {
        private readonly ILogger logger;
        private readonly IPoeApiService poeApiService;

        public StaticItemCategoryService(ILogger logger,
            IPoeApiService poeApiService)
        {
            this.logger = logger;
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
