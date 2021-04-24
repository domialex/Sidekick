using System.Text.RegularExpressions;

namespace Sidekick.Application.Game.Items.Parser.Tokenizers
{
    public class ItemNameTokenMatch
    {
        public bool IsMatch { get; set; }

        public ItemNameTokenType TokenType { get; set; }

        public Match Match { get; set; }
    }
}
