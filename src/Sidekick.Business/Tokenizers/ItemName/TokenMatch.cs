using System.Text.RegularExpressions;

namespace Sidekick.Business.Tokenizers.ItemName
{
    public class TokenMatch
    {
        public bool IsMatch { get; set; }

        public TokenType TokenType { get; set; }

        public Match Match { get; set; }
    }
}
