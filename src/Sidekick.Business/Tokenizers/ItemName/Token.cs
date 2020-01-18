namespace Sidekick.Business.Tokenizers.ItemName
{
    public enum TokenType
    {
        Set,
        If,
        Name,
        EndOfItem
    }

    public class Token
    {
        public Token(TokenType tokenType, TokenMatch value)
        {
            TokenType = tokenType;
            Match = value;
        }

        public TokenType TokenType { get; set; }

        public TokenMatch Match { get; set; }
    }
}
