using MediatR;
using Sidekick.Domain.Game.Items.Metadatas.Models;

namespace Sidekick.Domain.Game.Items.Commands
{
    /// <summary>
    /// Parses the item header information
    /// </summary>
    public class ParseItemHeaderCommand : ICommand<IItemMetadata>
    {
        /// <summary>
        /// Parses the item header information
        /// </summary>
        /// <param name="parsingItem">The item to parse</param>
        public ParseItemHeaderCommand(ParsingItem parsingItem)
        {
            ParsingItem = parsingItem;
        }

        /// <summary>
        /// The item to parse
        /// </summary>
        public ParsingItem ParsingItem { get; }
    }
}
