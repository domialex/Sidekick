using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Trades.Results;
using Sidekick.Localization.Prices;
using Sidekick.UI.Items;

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
            ItemLevel = result.Item.ItemLevel.ToString();
            Age = GetHumanReadableTimeSpan(result.Listing.Indexed);
            Item = result;

            if (Item.Item.Requirements == null)
            {
                Item.Item.Requirements = new List<LineContent>();
            }
            foreach (var requirement in Item.Item.Requirements)
            {
                requirement.Name = $"{PriceResources.Requires} {requirement.Name}";
            }
            Item.Item.Requirements.Add(new LineContent()
            {
                DisplayMode = 0,
                Name = PriceResources.ItemLevel,
                Values = new List<LineContentValue>
                {
                    new LineContentValue()
                    {
                        Type = LineContentType.Simple,
                        Value = Item.Item.ItemLevel.ToString(),
                    }
                },
                Order = -1,
            });
            Item.Item.Requirements = Item.Item.Requirements.OrderBy(x => x.Order).ToList();
        }

        public SearchResult Item { get; set; }

        public string Color => Item?.Item.Rarity.GetColor();

        public string AccountName { get; set; }

        public string CharacterName { get; set; }

        public string Amount { get; set; }
        public string Currency { get; set; }
        public string CurrencyUrl { get; set; }

        public string ItemLevel { get; set; }

        public string Age { get; set; }

        public List<LineContent> Requirements { get; set; }

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
