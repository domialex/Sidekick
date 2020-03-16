using Sidekick.Business.Apis.Poe.Models;
using System.Collections.Generic;

namespace Sidekick.Business.Categories
{
    public interface IStatsDataService
    {
        List<AttributeCategory> Categories { get; }
    }
}
