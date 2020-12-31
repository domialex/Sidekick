using MediatR;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Trade.Commands
{
    /// <summary>
    /// Price checks an item
    /// </summary>
    public class PriceCheckItemCommand : ICommand<bool>
    {
        /// <summary>
        /// Price checks the item under the cursor inside Path of Exile
        /// </summary>
        public PriceCheckItemCommand()
        {
        }

        /// <summary>
        /// Price checks the item passed as an argument
        /// </summary>
        /// <param name="item">The item to price check</param>
        public PriceCheckItemCommand(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// The item to price check
        /// </summary>
        public Item Item { get; }
    }
}
