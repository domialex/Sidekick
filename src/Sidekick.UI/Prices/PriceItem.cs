using System;
using Sidekick.Business.Trades.Results;
using Sidekick.Localization.Prices;

namespace Sidekick.UI.Prices
{
    public partial class PriceItem
    {
        public PriceItem(SearchResult result)
        {
            AccountName = result.Listing.Account.Name;
            CharacterName = result.Listing.Account.LastCharacterName;
            if (result.Listing.Price != null)
            {
                if (result.Listing.Price.Amount % 1 == 0)
                {
                    Amount = result.Listing.Price.Amount.ToString("N0");
                }
                else
                {
                    Amount = result.Listing.Price.Amount.ToString("N1");
                }
                Currency = result.Listing.Price.Currency;
            }
            ItemLevel = result.Item.Ilvl.ToString();
            Age = GetHumanReadableTimeSpan(result.Listing.Indexed);
            Item = result;
        }

        public SearchResult Item { get; set; }

        public string AccountName { get; set; }

        public string CharacterName { get; set; }

        public string Amount { get; set; }
        public string Currency { get; set; }
        public string CurrencyUrl { get; set; }

        public string ItemLevel { get; set; }

        public string Age { get; set; }

        private string GetHumanReadableTimeSpan(DateTimeOffset time)
        {
            var span = DateTimeOffset.Now - time;

            if (span.Days > 1) return string.Format(PriceResources.Age_Days, span.Days);
            if (span.Days == 1) return string.Format(PriceResources.Age_Day, span.Days);
            if (span.Hours > 1) return string.Format(PriceResources.Age_Hours, span.Hours);
            if (span.Hours == 1) return string.Format(PriceResources.Age_Hour, span.Hours);
            if (span.Minutes > 1) return string.Format(PriceResources.Age_Minutes, span.Minutes);
            if (span.Minutes == 1) return string.Format(PriceResources.Age_Minute, span.Minutes);
            if (span.Seconds > 10) return string.Format(PriceResources.Age_Seconds, span.Seconds);

            return PriceResources.Age_Now;
        }
    }
}
