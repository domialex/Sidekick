namespace Sidekick.Business.Trades.Results
{
    public class ListingResult
    {
        public string Id { get; set; }
        public Listing Listing { get; set; }
        public ItemListing Item { get; set; }
        public bool Gone { get; set; }
    }
}
