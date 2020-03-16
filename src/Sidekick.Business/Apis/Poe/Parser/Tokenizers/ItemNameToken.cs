namespace Sidekick.Business.Tokenizers.ItemName
{
    public class ItemNameToken
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
