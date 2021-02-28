using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Domain.Game.Items
{
    /// <summary>
    /// Stores data about the state of the parsing process for the item
    /// </summary>
    public class ParsingItem
    {
        private const string SEPARATOR_PATTERN = "--------";

        /// <summary>
        /// Stores data about the state of the parsing process for the item
        /// </summary>
        /// <param name="text">The original text of the item</param>
        public ParsingItem(string text)
        {
            Text = text;

            Blocks = text
                .Split(SEPARATOR_PATTERN, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new ParsingBlock(x))
                .ToList();
        }

        /// <summary>
        /// Item sections seperated by dashes when copying an item in-game.
        /// </summary>
        public List<ParsingBlock> Blocks { get; }

        /// <summary>
        /// The original text of the item
        /// </summary>
        public string Text { get; }
    }
}
