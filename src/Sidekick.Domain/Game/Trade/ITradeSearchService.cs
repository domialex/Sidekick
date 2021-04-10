using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Domain.Game.Trade
{
    public interface ITradeSearchService
    {
        Task<TradeSearchResult<string>> SearchBulk(Item item);
        Task<TradeSearchResult<string>> Search(Item item, PropertyFilters propertyFilters = null, ModifierFilters modifierFilters = null);
        Task<List<TradeItem>> GetResults(string queryId, List<string> ids, ModifierFilters modifierFilters = null);
    }
}
