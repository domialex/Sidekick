using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Categories
{
    public class StaticItemCategoryService : IStaticItemCategoryService, IOnInit, IDisposable
    {
        private readonly IPoeApiClient poeApiClient;

        public StaticItemCategoryService(IPoeApiClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<StaticItemCategory> Categories { get; private set; }
        public Dictionary<string, string> Lookup { get; private set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<StaticItemCategory>();
            Lookup = ToLookup();
        }

        public void Dispose()
        {
            Categories = null;
        }

        private Dictionary<string, string> ToLookup()
        {
            return Categories.Where(x => !x.Id.StartsWith("Maps")).SelectMany(x => x.Entries).ToDictionary(key => key.Text, value => value.Id);
        }
    }
}
