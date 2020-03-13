using System;
using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueService
    {
        List<League> Leagues { get; }

        event Action OnNewLeagues;
    }
}
