using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEPriceInfoAPI
{
    public class PriceInfo
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string Currency { get; set; }
        [JsonProperty(PropertyName = "pred_confidence_score")]
        public double PredictionConfidenceScore { get; set; }
        [JsonProperty(PropertyName = "warning_msg")]
        public string WarningMessage { get; set; }
        [JsonProperty(PropertyName = "error")]
        public int ErrorCode { get; set; }
        public string ItemText { get; set; }
    }
}
