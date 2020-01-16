using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Leagues.Models;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueService
    {
        List<League> Leagues { get; }
        League SelectedLeague { get; set; }

        Task Initialize();
    }
}