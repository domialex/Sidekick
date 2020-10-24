using System.Collections.Generic;
using MediatR;

namespace Sidekick.Domain.Leagues
{
    public class GetLeaguesQuery : IQuery<List<League>>
    {
    }
}
