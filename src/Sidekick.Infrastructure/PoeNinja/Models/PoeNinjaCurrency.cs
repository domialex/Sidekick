namespace Sidekick.Infrastructure.PoeNinja.Models
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
}
