using System.Text.RegularExpressions;

namespace Sidekick.Apis.Poe.Parser.Tokenizers
{
    public class ItemNameTokenMatch
    {
        public bool IsMatch { get; set; }

        public ItemNameTokenType TokenType { get; set; }

        public Match Match { get; set; }
    }
}
