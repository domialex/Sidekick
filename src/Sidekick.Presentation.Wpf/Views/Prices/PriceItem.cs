using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;
using Sidekick.Extensions;
using Sidekick.Localization.Prices;

namespace Sidekick.Views.Prices
{
    public partial class PriceItem
    {
        public PriceItem(TradeItem result)
        {
            Item = result;

            if (Item.Requirements != null)
            {
                var requires = new LineContent()
                {
                    DisplayMode = -1,
                    Name = PriceResources.Requires,
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = string.Join(", ", Item.Requirements.Select(x => { if (x.DisplayMode == 0) x.DisplayMode = -1; return x.Parsed; })),
                        }
                    }
                };

                Item.Requirements.Clear();
                Item.Requirements.Add(requires);
            }

            if (Item.ItemLevel > 0)
            {
                if (Item.Requirements == null)
                {
                    Item.Requirements = new List<LineContent>();
                }

                Item.Requirements.Add(new LineContent()
                {
                    DisplayMode = 0,
                    Name = PriceResources.ItemLevel,
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = Item.ItemLevel.ToString(),
                        }
                    },
                    Order = -1,
                });
            }

            if (Item.Requirements != null)
            {
                Item.Requirements = Item.Requirements.OrderBy(x => x.Order).ToList();
            }
        }

        public TradeItem Item { get; set; }

        public string Color => Item?.Rarity.GetColor();

        public string Amount
        {
            get
            {
                if (Item.Listing.Price == null)
                {
                    return null;
                }

                if (Item.Listing.Price.Amount % 1 == 0)
                {
                    return Item.Listing.Price.Amount.ToString("N0");
                }
                return Item.Listing.Price.Amount.ToString("N2");
            }
        }
        public string ImageUrl { get; set; }
        public string Age
        {
            get
            {
                var span = DateTimeOffset.Now - Item.Listing.Indexed;

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
}
