using System.Collections.Generic;

namespace Sidekick.Business.Trades.Results
{
    public class Property
    {
        public string Name { get; set; }
        public List<List<object>> Values { get; set; }
        public int DisplayMode { get; set; }
        public int Type { get; set; }
    }
}
