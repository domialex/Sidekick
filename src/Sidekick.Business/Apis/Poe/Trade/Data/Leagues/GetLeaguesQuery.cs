using System.Collections.Generic;
using MediatR;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Leagues
{
    public class GetLeaguesQuery : IRequest<List<League>>
    {
    }
}
