using MediatR;

namespace Sidekick.Domain.Game.Items.Commands
{
    /// <summary>
    /// Returns a ParsingItem object from an item text
    /// </summary>
    public class GetParsingItem : ICommand<ParsingItem>
    {
        /// <summary>
        /// Returns a ParsingItem object from an item text
        /// </summary>
        /// <param name="itemText">The text to parse</param>
        public GetParsingItem(string itemText)
        {
            ItemText = itemText;
        }

        /// <summary>
        /// The text to parse
        /// </summary>
        public string ItemText { get; }
    }
}
