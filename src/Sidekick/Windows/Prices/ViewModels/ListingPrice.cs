namespace Sidekick.Windows.Prices.ViewModels
{
    public partial class ListItem
    {
        public class ListingPrice
        {
            public decimal Amount { get; set; }
            public string Currency { get; set; }
            public string CurrencyUrl { get; set; }
        }
    }
}
