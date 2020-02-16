using System.Text.Json.Serialization;

namespace Sidekick.Business.Filters
{
    public class TradeFilters
    {
        public FilterOption Account { get; set; }
        [JsonPropertyName("indexed")]
        public FilterOption Listed { get; set; }
        public FilterValue Price { get; set; }
        [JsonPropertyName("sale_type")]
        public FilterOption SaleType { get; set; }
    }
}
