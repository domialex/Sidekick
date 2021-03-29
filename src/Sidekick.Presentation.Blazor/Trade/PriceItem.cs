using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Localization.Trade;
using Sidekick.Presentation.Blazor.Extensions;

namespace Sidekick.Presentation.Blazor.Trade
{
    public partial class PriceItem
    {
        public PriceItem(TradeItem result, TradeResources resources)
        {
            Item = result;

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

    }
}
