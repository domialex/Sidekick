using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public interface IStatDataService
    {
        List<StatDataCategory> Categories { get; }
    }
}
