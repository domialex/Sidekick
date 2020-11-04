using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Domain.Leagues
{
    /// <summary>
    /// Leagues repository
    /// </summary>
    public interface ILeagueRepository
    {
        /// <summary>
        /// Get all leagues from the database
        /// </summary>
        /// <returns>The list of leagues</returns>
        Task<List<League>> FindAll();

        /// <summary>
        /// Save a list of leagues in the database. The old database data is lost.
        /// </summary>
        /// <param name="leagues">The list of leagues to save</param>
        /// <returns></returns>
        Task SaveAll(List<League> leagues);
    }
}
