namespace Sidekick.Business.Tokenizers.ItemName
{
    public enum ItemNameTokenType
    {
        Set,
        If,
        Name,
        EndOfItem
    }

    public class ItemNameToken : IToken
    {
        public ItemNameToken(ItemNameTokenType tokenType, ItemNameTokenMatch value)
        {
            TokenType = tokenType;
            Match = value;
        }

        public ItemNameTokenType TokenType { get; set; }

        public ItemNameTokenMatch Match { get; set; }
    }
}
