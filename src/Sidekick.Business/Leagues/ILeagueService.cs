using Sidekick.Business.Leagues.Models;
using System.Collections.Generic;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueService
    {
        List<League> Leagues { get; }
    }
}
