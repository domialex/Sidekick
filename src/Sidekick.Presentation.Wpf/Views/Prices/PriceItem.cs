using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Presentation.Localization.Prices;
using Sidekick.Presentation.Wpf.Extensions;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    public partial class PriceItem
    {
        public PriceItem(TradeItem result)
        {
            Item = result;

            if (Item.Requirements != null)
            {
                var requirementValues = string.Join(", ", Item.Requirements.Select(x => x.Text.Replace(":", "")));
                var requires = new LineContent()
                {
                    Text = $"{PriceResources.Requires} {requirementValues}",
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = requirementValues,
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

                Item.Requirements = Item.Requirements.Prepend(new LineContent()
                {
                    Text = $"{PriceResources.ItemLevel}: {Item.ItemLevel}",
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = Item.ItemLevel.ToString(),
                        }
                    },
                })
                .ToList();
            }
        }

        public TradeItem Item { get; set; }

        public string Color => Item?.Rarity.GetColor();

        public string Amount
        {
            get
            {
                if (Item.Price == null)
                {
                    return null;
                }

                if (Item.Price.Amount % 1 == 0)
                {
                    return Item.Price.Amount.ToString("N0");
                }
                return Item.Price.Amount.ToString("N2");
            }
        }
        public string ImageUrl { get; set; }
        public string Age
        {
            get
            {
                var span = DateTimeOffset.Now - Item.Price.Date;

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
