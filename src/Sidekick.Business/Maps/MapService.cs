using Sidekick.Business.Categories;
using Sidekick.Business.Maps.Models;
using Sidekick.Core.Initialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Business.Maps
{
    public class MapService : IMapService, IOnInit, IOnReset
    {
        private readonly IStaticItemCategoryService staticItemCategoryService;

        public MapService(IStaticItemCategoryService staticItemCategoryService)
        {
            this.staticItemCategoryService = staticItemCategoryService;
        }

        public HashSet<string> MapNames { get; private set; }

        public Task OnInit()
        {
            var mapCategories = staticItemCategoryService.Categories.Where(c => MapTiers.TierIds.Contains(c.Id)).ToList();
            var allMapNames = new List<string>();

            foreach (var item in mapCategories)
            {
                allMapNames.AddRange(item.Entries.Select(c => c.Text));
            }

            MapNames = new HashSet<string>(allMapNames.Distinct());

            return Task.CompletedTask;
        }

        public Task OnReset()
        {
            MapNames = null;

            return Task.CompletedTask;
        }
    }
}
