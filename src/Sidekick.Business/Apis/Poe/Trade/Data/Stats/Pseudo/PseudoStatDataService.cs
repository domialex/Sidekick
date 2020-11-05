using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoStatDataService : IPseudoStatDataService
    {
        public List<PseudoDefinition> Definitions { get; set; } = new List<PseudoDefinition>();
    }
}
