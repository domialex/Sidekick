using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Localization.Prices;
using Sidekick.Presentation.Blazor.Extensions;

namespace Sidekick.Presentation.Blazor.Overlays.Prices
{
    public partial class PriceItem
    {
        private readonly PriceResources resources;

        public PriceItem(TradeItem result, PriceResources resources)
        {
            Item = result;
            this.resources = resources;

            if (Item.RequirementContents != null)
            {
                var requirementValues = string.Join(", ", Item.RequirementContents.Select(x => x.Text.Replace(":", "")));
                var requires = new LineContent()
                {
                    Text = $"{resources.Requires} {requirementValues}",
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = requirementValues,
                        }
                    }
                };

                Item.RequirementContents.Clear();
                Item.RequirementContents.Add(requires);
            }

            if (Item.Properties.ItemLevel > 0)
            {
                if (Item.RequirementContents == null)
                {
                    Item.RequirementContents = new List<LineContent>();
                }

                Item.RequirementContents = Item.RequirementContents.Prepend(new LineContent()
                {
                    Text = $"{resources.ItemLevel}: {Item.Properties.ItemLevel}",
                    Values = new List<LineContentValue>
                    {
                        new LineContentValue()
                        {
                            Type = LineContentType.Simple,
                            Value = Item.Properties.ItemLevel.ToString(),
                        }
                    },
                })
                .ToList();
            }
        }

        public TradeItem Item { get; set; }

        public string Color => Item?.Metadata.Rarity.GetColor();

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

                if (span.Days > 1) return string.Format(resources.Age_Days, span.Days);
                if (span.Days == 1) return string.Format(resources.Age_Day, span.Days);
                if (span.Hours > 1) return string.Format(resources.Age_Hours, span.Hours);
                if (span.Hours == 1) return string.Format(resources.Age_Hour, span.Hours);
                if (span.Minutes > 1) return string.Format(resources.Age_Minutes, span.Minutes);
                if (span.Minutes == 1) return string.Format(resources.Age_Minute, span.Minutes);
                if (span.Seconds > 10) return string.Format(resources.Age_Seconds, span.Seconds);

                return resources.Age_Now;
            }
        }
    }
}
