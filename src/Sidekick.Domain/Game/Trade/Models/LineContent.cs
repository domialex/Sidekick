using System.Collections.Generic;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class LineContent
    {
        public string Text { get; set; }

        public List<LineContentValue> Values { get; set; }
    }
}
