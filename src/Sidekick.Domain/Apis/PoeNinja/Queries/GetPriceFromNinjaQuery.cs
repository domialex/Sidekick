using MediatR;
using Sidekick.Domain.Apis.PoeNinja.Models;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Apis.PoeNinja.Queries
{
    /// <summary>
    /// Gets the price from Poe Ninja
    /// </summary>
    public class GetPriceFromNinjaQuery : IQuery<NinjaPrice>
    {
        /// <summary>
        /// Gets the price from Poe Ninja
        /// </summary>
        /// <param name="item">The item to get the price for</param>
        public GetPriceFromNinjaQuery(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// The item to get the price for
        /// </summary>
        public Item Item { get; }
    }
}
