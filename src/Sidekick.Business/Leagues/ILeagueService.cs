using Sidekick.Business.Apis.Poe.Models;
using System.Collections.Generic;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueService
    {
        List<League> Leagues { get; }
    }
}
