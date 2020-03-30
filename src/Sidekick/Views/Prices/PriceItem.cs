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
        public PriceItem(Result result)
        {
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
            }
            Age = GetHumanReadableTimeSpan(result.Listing.Indexed);
            Item = result;

            if (Item.Item.Requirements != null)
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
                            Value = string.Join(", ", Item.Item.Requirements.Select(x => { if (x.DisplayMode == 0) x.DisplayMode = -1; return x.Parsed; })),
                        }
                    }
                };

                Item.Item.Requirements.Clear();
                Item.Item.Requirements.Add(requires);
            }

            if (Item.Item.ItemLevel > 0)
            {
                if (Item.Item.Requirements == null)
                {
                    Item.Item.Requirements = new List<LineContent>();
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
            }

            Item.Item.Requirements = Item.Item.Requirements.OrderBy(x => x.Order).ToList();
        }

        public Result Item { get; set; }

        public string Color => Item?.Item.Rarity.GetColor();

        public string Amount { get; set; }
        public string ImageUrl { get; set; }
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
