using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class TradeItem : Item
    {
        public string Id { get; set; }
        public TradePrice Price { get; set; }

        public string Icon { get; set; }
        public string Note { get; set; }
        public List<LineContent> PropertyTexts { get; set; }
        public List<LineContent> Requirements { get; set; }
    }
}
