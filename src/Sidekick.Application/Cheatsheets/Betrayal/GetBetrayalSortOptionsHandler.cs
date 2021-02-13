using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets.Betrayal;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetBetrayalSortOptionsHandler : IQueryHandler<GetBetrayalSortOptionsQuery, Dictionary<string, string>>
    {
        private readonly BetrayalResources resources;

        public GetBetrayalSortOptionsHandler(
            BetrayalResources resources)
        {
            this.resources = resources;
        }

        public Task<Dictionary<string, string>> Handle(GetBetrayalSortOptionsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Dictionary<string, string>()
            {
                { "", resources.Sort_Alphabetical },
                { "value", resources.Sort_Value },
                { "transportation", resources.Sort_Transportation },
                { "fortification", resources.Sort_Fortification },
                { "research", resources.Sort_Research },
                { "intervention", resources.Sort_Intervention },
            });
        }
    }
}
