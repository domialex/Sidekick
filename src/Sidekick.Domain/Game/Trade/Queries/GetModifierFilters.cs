using MediatR;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Domain.Game.Trade.Queries
{
    /// <summary>
    /// Gets a list of modifier filters for a specific item
    /// </summary>
    public class GetModifierFilters : IQuery<ModifierFilters>
    {
        /// <summary>
        /// Gets a list of modifier filters for a specific item
        /// </summary>
        /// <param name="item">The item for which to get modifier filters</param>
        public GetModifierFilters(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// The item for which to get modifier filters
        /// </summary>
        public Item Item { get; }
    }
}
