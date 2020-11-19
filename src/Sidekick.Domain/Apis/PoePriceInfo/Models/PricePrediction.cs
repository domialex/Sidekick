namespace Sidekick.Domain.Apis.PoePriceInfo.Models
{
    public class PricePrediction
    {
        public double? Min { get; set; }

        public double? Max { get; set; }

        public string Currency { get; set; }

        public double ConfidenceScore { get; set; }
    }
}
