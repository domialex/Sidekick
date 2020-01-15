using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class MiscFilters
    {
        public FilterValue Quality { get; set; }

        [JsonProperty(PropertyName = "gem_level")]
        public FilterValue GemLevel { get; set; }

        [JsonProperty(PropertyName = "ilvl")]
        public FilterValue ItemLevel { get; set; }

        [JsonProperty(PropertyName = "talisman_art")]
        public FilterOption TalismanArt { get; set; }

        [JsonProperty(PropertyName = "alternate_art")]
        public FilterOption AlternateArt { get; set; }

        public FilterOption Identified { get; set; }

        public FilterOption Corrupted { get; set; }

        public FilterOption Crafted { get; set; }

        public FilterOption Enchanted { get; set; }

        public FilterOption Veiled { get; set; }

        public FilterOption Mirrored { get; set; }

        [JsonProperty(PropertyName = "elder_item")]
        public FilterOption ElderItem { get; set; }

        [JsonProperty(PropertyName = "hunter_item")]
        public FilterOption HunterItem { get; set; }

        [JsonProperty(PropertyName = "shaper_item")]
        public FilterOption ShaperItem { get; set; }

        [JsonProperty(PropertyName = "warlord_item")]
        public FilterOption WarlordItem { get; set; }

        [JsonProperty(PropertyName = "crusader_item")]
        public FilterOption CrusaderItem { get; set; }

        [JsonProperty(PropertyName = "redeemer_item")]
        public FilterOption RedeemerItem { get; set; }

        [JsonProperty(PropertyName = "synthesised_item")]
        public FilterOption SynthesisedItem { get; set; }
    }
}
