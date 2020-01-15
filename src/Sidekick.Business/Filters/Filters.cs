using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class Filters
    {
        [JsonProperty(PropertyName = "misc_filters")]
        public MiscFilter MiscFilters { get; set; } = new MiscFilter();

        [JsonProperty(PropertyName = "weapon_filters")]
        public WeaponFilter WeaponFilters { get; set; } = new WeaponFilter();

        [JsonProperty(PropertyName = "armour_filters")]
        public ArmorFilter ArmourFilter { get; set; } = new ArmorFilter();

        [JsonProperty(PropertyName = "socket_filters")]
        public SocketFilter SocketFilter { get; set; } = new SocketFilter();

        [JsonProperty(PropertyName = "req_filters")]
        public RequierementFilter RequierementFilter { get; set; } = new RequierementFilter();

        [JsonProperty(PropertyName = "type_filters")]
        public TypeFilter TypeFilter { get; set; } = new TypeFilter();

        [JsonProperty(PropertyName = "map_filters")]
        public MapFilter MapFilter { get; set; } = new MapFilter();
    }
}
