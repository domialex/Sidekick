using System.Text.RegularExpressions;

namespace Sidekick.Business.Tokenizers.ItemName
{
    public class ItemNameTokenMatch
    {
        public bool IsMatch { get; set; }

        public ItemNameTokenType TokenType { get; set; }

        public Match Match { get; set; }
    }
}
