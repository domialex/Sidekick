using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Domain.Leagues
{
    public interface ILeagueRepository
    {
        Task<List<League>> FindAll();
        Task Clear();
        Task SaveAll(List<League> leagues);
    }
}
