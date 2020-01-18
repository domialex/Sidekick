using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Tokenizers.ItemName
{
    public class Tokenizer
    {
        private Dictionary<TokenType, string> _tokenDefs;

        public Tokenizer()
        {
            _tokenDefs = new Dictionary<TokenType, string>();

            _tokenDefs.Add(TokenType.Set, "<<set:(?<LANG>\\w{1,2})>>");
            _tokenDefs.Add(TokenType.Name, "^((?!<).)+");
            _tokenDefs.Add(TokenType.If, "<(?:el)?if:(?<LANG>\\w{1,2})>{(?<NAME>\\s?((?!<).)+)}");
        }

        public IEnumerable<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();

            while (!string.IsNullOrWhiteSpace(input))
            {
                var match = FindMatch(ref input);
                if (match.IsMatch)
                {
                    tokens.Add(new Token(match.TokenType, match));
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

            tokens.Add(new Token(TokenType.EndOfItem, null));

            return tokens;
        }

        private TokenMatch FindMatch(ref string input)
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

                    return new TokenMatch()
                    {
                        IsMatch = true,
                        TokenType = def.Key,
                        Match = match
                    };
                }
            }

            return new TokenMatch() { IsMatch = false };
        }
    }
}
