using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.PoePriceInfo.Models
{
    public class PriceInfoResult
    {
        public double? Min { get; set; }

        public double? Max { get; set; }

        public string Currency { get; set; }

        //[JsonPropertyName("pred_explanation")]
        //public List<Dictionary<string, double>> Explanation { get; set; }

        [JsonPropertyName("pred_confidence_score")]
        public double ConfidenceScore { get; set; }

        [JsonPropertyName("warning_msg")]
        public string WarningMessage { get; set; }

        [JsonPropertyName("error")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_msg")]
        public string ErrorMessage { get; set; }
    }
}
