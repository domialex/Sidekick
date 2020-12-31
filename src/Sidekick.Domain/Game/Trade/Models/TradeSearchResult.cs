using System.Collections.Generic;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class TradeSearchResult<T>
    {
        public List<T> Result { get; set; }

        public string Id { get; set; }

        public int Total { get; set; }
    }
}
