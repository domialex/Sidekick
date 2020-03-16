using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Maps.Models;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Maps
{
    public class MapService : IMapService, IOnAfterInit
    {
        private readonly IStaticDataService staticDataService;

        public MapService(IStaticDataService staticDataService)
        {
            this.staticDataService = staticDataService;
        }

        public HashSet<string> MapNames { get; private set; }

        public Task OnAfterInit()
        {
            MapNames = new HashSet<string>(staticDataService.Categories
                .Where(c => MapTiers.TierIds.Contains(c.Id))
                .SelectMany(x => x.Entries)
                .Select(x => x.Text)
                .Distinct()
                .ToList());

            return Task.CompletedTask;
        }
    }
}
