using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Categories
{
    public class AttributeCategoryService : IAttributeCategoryService, IOnInit
    {
        private readonly IPoeApiClient poeApiClient;

        public AttributeCategoryService(IPoeApiClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<AttributeCategory> Categories { get; private set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<AttributeCategory>();
        }
    }
}
