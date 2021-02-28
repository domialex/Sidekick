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

            WholeSections = Blocks.Select(x => x.Text).ToArray();
            SplitSections = Blocks.Select(block => block.Lines.Select(x => x.Text).ToArray()).ToArray();
        }

        /// <summary>
        /// Item sections seperated by dashes when copying an item in-game.
        /// </summary>
        public List<ParsingBlock> Blocks { get; }

        [Obsolete]
        public string[][] SplitSections { get; }

        [Obsolete]
        public string[] WholeSections { get; }

        [Obsolete]
        public string[] HeaderSection => SplitSections[0];

        [Obsolete]
        public string MapPropertiesSection => WholeSections[1];

        [Obsolete]
        public string NameLine => HeaderSection.Length > 1 ? HeaderSection[1] : string.Empty;

        [Obsolete]
        public string TypeLine => HeaderSection.Length > 2 ? HeaderSection[2] : string.Empty;

        /// <summary>
        /// The original text of the item
        /// </summary>
        public string Text { get; }
    }
}
