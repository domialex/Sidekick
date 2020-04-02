using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Modifier
    {
        public string Id { get; set; }

        public string Tier { get; set; }

        public string TierName { get; set; }

        public string Text { get; set; }

        public List<double> Values { get; set; } = new List<double>();
    }
}
