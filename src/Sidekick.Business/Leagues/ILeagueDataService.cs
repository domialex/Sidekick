using System.Collections.Generic;
using Sidekick.Domain.Leagues;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueDataService
    {
        List<League> Leagues { get; }

        void Initialize(List<League> leagues);
    }
}
