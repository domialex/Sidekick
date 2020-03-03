namespace Sidekick.Business.Parsers.Models
{
    struct ItemProperties
    {
        public bool IsIdentified { get; set; }
        public bool HasQuality { get; set; }
        public bool IsCorrupted { get; set; }
        public bool IsMap { get; set; }
        public bool IsBlighted { get; set; }
        public bool IsOrgan { get; set; }
        public bool HasNote { get; set; }
    }
}
