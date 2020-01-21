using System;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class PoeNinjaCurrency : PoeNinjaResult
    {
        public string CurrencyTypeName { get; set; }

        public PoeNinjaExchange Pay { get; set; }

        public PoeNinjaExchange Receive { get; set; }

        public SparkLine PaySparkLine { get; set; }

        public SparkLine ReceiveSparkLine { get; set; }

        public SparkLine LowConfidencePaySparkLine { get; set; }

        public SparkLine LowConfidenceReviceSparkLine { get; set; }

        public string DetailsId { get; set; }
    }

    public class PoeNinjaExchange
    {
        public int Id { get; set; }

        [JsonPropertyName("league_id")]
        public int LeagueId { get; set; }

        [JsonPropertyName("pay_currency_id")]
        public int PayCurrencyId { get; set; }

        [JsonPropertyName("get_currency_id")]
        public int GetCurrencyId { get; set; }

        [JsonPropertyName("sample_time_utc")]
        public DateTime SampleTimeUtc { get; set; }

        public int Count { get; set; }

        public double Value { get; set; }

        [JsonPropertyName("data_point_count")]
        public int DataPointCount { get; set; }

        [JsonPropertyName("includes_secondary")]
        public bool IncludesSecondary { get; set; }

    }
}
