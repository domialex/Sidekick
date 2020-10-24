using System.Collections.Generic;
using Sidekick.Domain.Leagues;

namespace Sidekick.Business.Leagues
{
    public class LeagueDataService : ILeagueDataService
    {
        public List<League> Leagues { get; private set; } = new List<League>();

        public void Initialize(List<League> leagues)
        {
            Leagues = leagues;
        }
    }
}
