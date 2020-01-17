using System;

namespace Sidekick.Business.Trades.Results
{
    public class Listing
    {
        public DateTime Indexed { get; set; }
        public Account Account { get; set; }
        public Price Price { get; set; }
    }
}
