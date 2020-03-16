using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Tokenizers.ItemName
{
    public class ItemNameTokenizer
    {
        private Dictionary<ItemNameTokenType, string> _tokenDefs;

        public ItemNameTokenizer()
        {
            _tokenDefs = new Dictionary<ItemNameTokenType, string>();

            _tokenDefs.Add(ItemNameTokenType.Set, "<<set:(?<LANG>\\w{1,2})>>");
            _tokenDefs.Add(ItemNameTokenType.Name, "^((?!<).)+");
            _tokenDefs.Add(ItemNameTokenType.If, "<(?:el)?if:(?<LANG>\\w{1,2})>{(?<NAME>\\s?((?!<).)+)}");
        }

        private IEnumerable<ItemNameToken> GetTokens(string input)
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

        public string CleanString(string input)
        {
            var langs = new List<string>();
            var tokens = GetTokens(input);
            var output = new StringBuilder();

            foreach (var token in tokens)
            {
                if (token.TokenType == ItemNameTokenType.Set)
                {
                    langs.Add(token.Match.Match.Groups["LANG"].Value);
                }
                else if (token.TokenType == ItemNameTokenType.Name)
                {
                    output.Append(token.Match.Match.Value);
                }
                else if (token.TokenType == ItemNameTokenType.If)
                {
                    var lang = token.Match.Match.Groups["LANG"].Value;
                    if (langs.Contains(lang))
                    {
                        output.Append(token.Match.Match.Groups["NAME"].Value);
                    }
                }
            }

            return output.ToString();
        }
    }
}
