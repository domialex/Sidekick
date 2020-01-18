using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Tokenizers.ItemName
{
    public class ItemNameTokenizer : ITokenizer
    {
        private Dictionary<ItemNameTokenType, string> _tokenDefs;

        public ItemNameTokenizer()
        {
            _tokenDefs = new Dictionary<ItemNameTokenType, string>();

            _tokenDefs.Add(ItemNameTokenType.Set, "<<set:(?<LANG>\\w{1,2})>>");
            _tokenDefs.Add(ItemNameTokenType.Name, "^((?!<).)+");
            _tokenDefs.Add(ItemNameTokenType.If, "<(?:el)?if:(?<LANG>\\w{1,2})>{(?<NAME>\\s?((?!<).)+)}");
        }

        public IEnumerable<IToken> Tokenize(string input)
        {
            var tokens = new List<ItemNameToken>();

            while (!string.IsNullOrWhiteSpace(input))
            {
                var match = FindMatch(ref input);
                if (match.IsMatch)
                {
                    tokens.Add(new ItemNameToken(match.TokenType, match));
                }
                else
                {
                    if (Regex.IsMatch(input, "^\\s+"))
                    {
                        input = input.TrimStart();
                    }
                    else
                    {
                        throw new System.Exception("Failed to parse item name");
                    }
                }
            }

            tokens.Add(new ItemNameToken(ItemNameTokenType.EndOfItem, null));

            return tokens;
        }

        private ItemNameTokenMatch FindMatch(ref string input)
        {
            foreach (var def in _tokenDefs)
            {
                var match = Regex.Match(input, def.Value);
                if (match.Success)
                {
                    if (match.Length != input.Length)
                        input = input.Substring(match.Length);
                    else
                        input = string.Empty;

                    return new ItemNameTokenMatch()
                    {
                        IsMatch = true,
                        TokenType = def.Key,
                        Match = match
                    };
                }
            }

            return new ItemNameTokenMatch() { IsMatch = false };
        }
    }
}
