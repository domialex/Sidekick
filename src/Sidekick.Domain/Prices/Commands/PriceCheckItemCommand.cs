using MediatR;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Prices.Commands
{
    /// <summary>
    /// Price checks an item
    /// </summary>
    public class PriceCheckItemCommand : ICommand<bool>
    {
        /// <summary>
        /// Price checks the item
        /// </summary>
        /// <param name="item">The item to price check. If null, it will try to price check the item under the cursor inside Path of Exile.</param>
        public PriceCheckItemCommand(Item item = null)
        {
            Item = item;
        }

        public Item Item { get; }
    }
}
