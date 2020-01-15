using Sidekick.Business.Filters;
using Sidekick.Business.Parsers.Types;

namespace Sidekick.Business.Parsers.Models
{
    public class EquippableItem : Item
    {
        public string Quality { get; set; }
        public string ItemLevel { get; set; }
        public InfluenceType Influence { get; set; }
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
    }
}
