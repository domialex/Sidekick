using System.Text.Json.Serialization;

namespace Sidekick.Business.Filters
{
    public class Filters
    {
        [JsonPropertyName("misc_filters")]
        public MiscFilter MiscFilters { get; set; } = new MiscFilter();

        [JsonPropertyName("weapon_filters")]
        public WeaponFilter WeaponFilters { get; set; } = new WeaponFilter();

        [JsonPropertyName("armour_filters")]
        public ArmorFilter ArmourFilter { get; set; } = new ArmorFilter();

        [JsonPropertyName("socket_filters")]
        public SocketFilter SocketFilter { get; set; } = new SocketFilter();

        [JsonPropertyName("req_filters")]
        public RequierementFilter RequierementFilter { get; set; } = new RequierementFilter();

        [JsonPropertyName("type_filters")]
        public TypeFilter TypeFilter { get; set; } = new TypeFilter();

        [JsonPropertyName("map_filters")]
        public MapFilter MapFilter { get; set; } = new MapFilter();
    }
}
