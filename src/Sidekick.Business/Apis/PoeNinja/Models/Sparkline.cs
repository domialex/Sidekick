using System.Collections.Generic;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class SparkLine
    {
        public double TotalChange { get; set; }

        public List<double?> Data { get; set; }
    }
}
