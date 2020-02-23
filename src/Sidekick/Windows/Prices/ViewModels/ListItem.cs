using System;
using Sidekick.Business.Trades.Results;

namespace Sidekick.Windows.Prices.ViewModels
{
    public partial class ListItem
    {
        public ListItem(ListingResult item, string currencyUrl)
        {
            AccountName = item.Listing.Account.Name;
            CharacterName = item.Listing.Account.LastCharacterName;
            if (item.Listing.Price != null)
            {
                Price = new ListingPrice
                {
                    Amount = item.Listing.Price.Amount,
                    Currency = item.Listing.Price.Currency,
                    CurrencyUrl = currencyUrl
                };
            }
            ItemLevel = item.Item.Ilvl.ToString();
            Age = GetHumanReadableTimeSpan(item.Listing.Indexed);
        }

        public ListingResult Item { get; set; }

        public string AccountName { get; set; }

        public string CharacterName { get; set; }

        public ListingPrice Price { get; set; }

        public string ItemLevel { get; set; }

        public string Age { get; set; }

        private string GetHumanReadableTimeSpan(DateTime time)
        {
            var span = DateTime.Now - time.ToLocalTime();

            if (span.Days > 1) return $"{span.Days} days";
            if (span.Days == 1) return $"{span.Days} day";
            if (span.Hours > 1) return $"{span.Hours} hours";
            if (span.Hours == 1) return $"{span.Hours} hour";
            if (span.Minutes > 1) return $"{span.Minutes} minutes";
            if (span.Minutes == 1) return $"{span.Minutes} minute";
            if (span.Seconds > 10) return $"{span.Seconds} seconds";

            return "now";
        }
    }
}
