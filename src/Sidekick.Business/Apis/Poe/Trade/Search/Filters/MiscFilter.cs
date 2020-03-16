using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class MiscFilter
    {
        public FilterValue Quality { get; set; }

        [JsonPropertyName("gem_level")]
        public FilterValue GemLevel { get; set; }

        [JsonPropertyName("ilvl")]
        public FilterValue ItemLevel { get; set; }

        [JsonPropertyName("talisman_art")]
        public FilterOption TalismanArt { get; set; }

        [JsonPropertyName("alternate_art")]
        public FilterOption AlternateArt { get; set; }

        public FilterOption Identified { get; set; }

        public FilterOption Corrupted { get; set; }

        public FilterOption Crafted { get; set; }

        public FilterOption Enchanted { get; set; }

        public FilterOption Veiled { get; set; }

        public FilterOption Mirrored { get; set; }

        [JsonPropertyName("elder_item")]
        public FilterOption ElderItem { get; set; }

        [JsonPropertyName("hunter_item")]
        public FilterOption HunterItem { get; set; }

        [JsonPropertyName("shaper_item")]
        public FilterOption ShaperItem { get; set; }

        [JsonPropertyName("warlord_item")]
        public FilterOption WarlordItem { get; set; }

        [JsonPropertyName("crusader_item")]
        public FilterOption CrusaderItem { get; set; }

        [JsonPropertyName("redeemer_item")]
        public FilterOption RedeemerItem { get; set; }

        [JsonPropertyName("synthesised_item")]
        public FilterOption SynthesisedItem { get; set; }
    }
}
