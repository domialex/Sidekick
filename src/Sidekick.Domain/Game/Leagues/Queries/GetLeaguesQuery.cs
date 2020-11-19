using System.Collections.Generic;
using MediatR;

namespace Sidekick.Domain.Game.Leagues.Queries
{
    /// <summary>
    /// Query to get a list of currently available leagues
    /// </summary>
    public class GetLeaguesQuery : IQuery<List<League>>
    {
        /// <summary>
        /// Query to get a list of currently available leagues
        /// </summary>
        /// <param name="useCache">If true, the leagues will be fetched from the cache if possible; if false, from the API</param>
        public GetLeaguesQuery(bool useCache)
        {
            UseCache = useCache;
        }

        /// <summary>
        /// If true, the leagues will be fetched from the cache if possible; if false, from the API
        /// </summary>
        public bool UseCache { get; }
    }
}
