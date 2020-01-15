using System.Collections.Generic;

namespace Sidekick.Business.Filters
{
    public class Exchange
    {
        public List<string> Want { get; set; } = new List<string>();

        public List<string> Have { get; set; } = new List<string>();

        public string Status = "online";
    }
}
