using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Leagues;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Leagues
{
    public class GetLeaguesQueryHandler : IQueryHandler<GetLeaguesQuery, List<League>>
    {
        private readonly IPoeTradeClient poeTradeClient;

        public GetLeaguesQueryHandler(IPoeTradeClient poeTradeClient)
        {
            this.poeTradeClient = poeTradeClient;
        }

        public async Task<List<League>> Handle(GetLeaguesQuery request, CancellationToken cancellationToken)
        {
            return await poeTradeClient.Fetch<League>();
        }
    }
}
