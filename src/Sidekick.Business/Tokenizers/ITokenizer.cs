using System.Collections.Generic;

namespace Sidekick.Business.Tokenizers
{
    public interface ITokenizer
    {
        IEnumerable<IToken> Tokenize(string input);
    }
}
