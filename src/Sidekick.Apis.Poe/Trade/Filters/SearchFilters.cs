using System.Text.Json.Serialization;

namespace Sidekick.Apis.Poe.Trade.Filters
{
    public class SearchFilters
    {
        [JsonPropertyName("misc_filters")]
        public MiscFilterGroup MiscFilters { get; set; } = new MiscFilterGroup();

        [JsonPropertyName("weapon_filters")]
        public WeaponFilterGroup WeaponFilters { get; set; } = new WeaponFilterGroup();

        [JsonPropertyName("armour_filters")]
        public ArmorFilterGroup ArmourFilters { get; set; } = new ArmorFilterGroup();

        [JsonPropertyName("socket_filters")]
        public SocketFilterGroup SocketFilters { get; set; } = new SocketFilterGroup();

        [JsonPropertyName("req_filters")]
        public RequirementFilterGroup RequirementFilters { get; set; } = new RequirementFilterGroup();

        [JsonPropertyName("type_filters")]
        public TypeFilterGroup TypeFilters { get; set; } = new TypeFilterGroup();

        [JsonPropertyName("map_filters")]
        public MapFilterGroup MapFilters { get; set; } = new MapFilterGroup();

        [JsonPropertyName("trade_filters")]
        public TradeFilterGroup TradeFilters { get; set; } = new TradeFilterGroup();
    }
}
