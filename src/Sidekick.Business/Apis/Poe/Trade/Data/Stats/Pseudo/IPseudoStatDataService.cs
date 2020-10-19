using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public interface IPseudoStatDataService
    {
        List<PseudoDefinition> Definitions { get; set; }
    }
}
