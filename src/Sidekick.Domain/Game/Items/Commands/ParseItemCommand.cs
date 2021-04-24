using MediatR;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Items.Commands
{
    /// <summary>
    /// Parses the item text into an Item object
    /// </summary>
    public class ParseItemCommand : ICommand<Item>
    {
        /// <summary>
        /// Parses the item text into an Item object
        /// </summary>
        /// <param name="itemText">The text to parse</param>
        public ParseItemCommand(string itemText)
        {
            ItemText = itemText;
        }

        /// <summary>
        /// The text to parse
        /// </summary>
        public string ItemText { get; }
    }
}
