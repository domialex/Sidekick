using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class TradeFilter
    {
        public FilterOption Account { get; set; }

        [JsonPropertyName("indexed")]
        public FilterOption Listed { get; set; }

        public FilterValue Price { get; set; }

        [JsonPropertyName("sale_type")]
        public FilterOption SaleType { get; set; }
    }
}
