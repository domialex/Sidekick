using System.Collections.Generic;
using MediatR;

namespace Sidekick.Domain.Leagues
{
    public class GetLeaguesQuery : IQuery<List<League>>
    {
        public GetLeaguesQuery(bool useCache)
        {
            UseCache = useCache;
        }

        public bool UseCache { get; }
    }
}
