using System.Collections.Generic;

namespace Sidekick.Apis.Poe.Trade.Requests
{
    public class Exchange
    {
        public List<string> Want { get; set; } = new List<string>();

        public List<string> Have { get; set; } = new List<string>();

        public Status Status { get; set; } = new Status();
    }
}
