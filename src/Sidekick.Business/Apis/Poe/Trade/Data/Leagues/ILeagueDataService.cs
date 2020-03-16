using System;
using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Leagues
{
    public interface ILeagueDataService
    {
        List<League> Leagues { get; }

        event Action OnNewLeagues;
    }
}
