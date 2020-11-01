using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Leagues;

namespace Sidekick.Infrastructure.Poe.Leagues
{
    public class GetLeaguesQueryHandler : IQueryHandler<GetLeaguesQuery, List<League>>
    {
        private readonly IPoeTradeClient poeTradeClient;
        private readonly ILeagueRepository leagueRepository;

        public GetLeaguesQueryHandler(
            IPoeTradeClient poeTradeClient,
            ILeagueRepository leagueRepository)
        {
            this.poeTradeClient = poeTradeClient;
            this.leagueRepository = leagueRepository;
        }

        public async Task<List<League>> Handle(GetLeaguesQuery request, CancellationToken cancellationToken)
        {
            if (request.UseCache)
            {
                var cacheLeagues = await leagueRepository.FindAll();
                if (cacheLeagues.Count != 0)
                {
                    return cacheLeagues;
                }
            }

            var leagues = await poeTradeClient.Fetch<League>("data/leagues");
            await leagueRepository.SaveAll(leagues);
            return leagues;
        }
    }
}
