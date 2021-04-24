using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Metamorph;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetMetamorphCheatsheetHandler : IQueryHandler<GetMetamorphCheatsheetQuery, MetamorphLeague>
    {
        private readonly MetamorphResources resources;

        public GetMetamorphCheatsheetHandler(
            MetamorphResources resources)
        {
            this.resources = resources;
        }

        public Task<MetamorphLeague> Handle(GetMetamorphCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var catalysts = new List<MetamorphCatalyst>()
            {
                new MetamorphCatalyst(resources.AbrasiveCatalyst, resources.AbrasiveCatalystEffect, RewardValue.Low, "Abrasive_Catalyst.png"), // 2
                new MetamorphCatalyst(resources.FertileCatalyst, resources.FertileCatalystEffect, RewardValue.High, "Fertile_Catalyst.png"), // 25
                new MetamorphCatalyst(resources.ImbuedCatalyst, resources.ImbuedCatalystEffect, RewardValue.Low, "Imbued_Catalyst.png"), // 1
                new MetamorphCatalyst(resources.IntrinsicCatalyst, resources.IntrinsicCatalystEffect, RewardValue.Low, "Intrinsic_Catalyst.png"),
                new MetamorphCatalyst(resources.PrismaticCatalyst, resources.PrismaticCatalystEffect, RewardValue.High, "Prismatic_Catalyst.png"), // 24
                new MetamorphCatalyst(resources.TemperingCatalyst, resources.TemperingCatalystEffect, RewardValue.Medium, "Tempering_Catalyst.png"), // 12
                new MetamorphCatalyst(resources.TurbulentCatalyst, resources.TurbulentCatalystEffect, RewardValue.Medium, "Turbulent_Catalyst.png"), // 5
            };

            return Task.FromResult(new MetamorphLeague()
            {
                Catalysts = catalysts
            });
        }
    }
}
