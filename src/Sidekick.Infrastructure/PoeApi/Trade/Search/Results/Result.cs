namespace Sidekick.Infrastructure.PoeApi.Trade.Search.Results
{
    public class Result
    {
        public string Id { get; set; }
        public Listing Listing { get; set; }
        public ResultItem Item { get; set; }
    }
}
