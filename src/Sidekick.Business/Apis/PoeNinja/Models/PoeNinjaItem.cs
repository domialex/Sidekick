using System.Collections.Generic;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class PoeNinjaItem : PoeNinjaResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public int MapTier { get; set; }

        public int LevelRequired { get; set; }

        public string BaseType { get; set; }

        public string Variant { get; set; }


        public string ProphecyText { get; set; }

        public string ArtFilename { get; set; }

        public int Links { get; set; }

        public int ItemClass { get; set; }

        public SparkLine SparkLine { get; set; }

        public SparkLine LowConfidenceSparkLine { get; set; }

        public List<Modifier> ImplicitModifiers { get; set; }

        public List<Modifier> ExplicitModifiers { get; set; }

        public string FlavourText { get; set; }

        public bool Corrupted { get; set; }

        public int GemLevel { get; set; }

        public int GemQuality { get; set; }

        public string ItemType { get; set; }

        public double ChaosValue { get; set; }

        public double ExaltedValue { get; set; }

        public int Count { get; set; }

        public string DetailsId { get; set; }
    }
}
