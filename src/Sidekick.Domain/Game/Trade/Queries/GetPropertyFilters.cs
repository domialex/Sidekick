using MediatR;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Domain.Game.Trade.Queries
{
    /// <summary>
    /// Gets a list of property filters for a specific item
    /// </summary>
    public class GetPropertyFilters : IQuery<PropertyFilters>
    {
        /// <summary>
        /// Gets a list of property filters for a specific item
        /// </summary>
        /// <param name="item">The item for which to get property filters</param>
        public GetPropertyFilters(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// The item for which to get property filters
        /// </summary>
        public Item Item { get; }
    }
}
