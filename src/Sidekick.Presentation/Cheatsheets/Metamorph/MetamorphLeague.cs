using System.Collections.Generic;
using Sidekick.Presentation.Localization.Leagues.Metamorph;

namespace Sidekick.Presentation.Cheatsheets.Metamorph
{
    public class MetamorphLeague
    {
        public MetamorphLeague()
        {
            Catalysts = new List<MetamorphCatalyst>()
            {
                new MetamorphCatalyst(MetamorphResources.AbrasiveCatalyst, MetamorphResources.AbrasiveCatalystEffect, RewardValue.Low, "Abrasive_Catalyst.png"), // 2
                new MetamorphCatalyst(MetamorphResources.FertileCatalyst, MetamorphResources.FertileCatalystEffect, RewardValue.High, "Fertile_Catalyst.png"), // 25
                new MetamorphCatalyst(MetamorphResources.ImbuedCatalyst, MetamorphResources.ImbuedCatalystEffect, RewardValue.Low, "Imbued_Catalyst.png"), // 1
                new MetamorphCatalyst(MetamorphResources.IntrinsicCatalyst, MetamorphResources.IntrinsicCatalystEffect, RewardValue.Low, "Intrinsic_Catalyst.png"),
                new MetamorphCatalyst(MetamorphResources.PrismaticCatalyst, MetamorphResources.PrismaticCatalystEffect, RewardValue.High, "Prismatic_Catalyst.png"), // 24
                new MetamorphCatalyst(MetamorphResources.TemperingCatalyst, MetamorphResources.TemperingCatalystEffect, RewardValue.Medium, "Tempering_Catalyst.png"), // 12
                new MetamorphCatalyst(MetamorphResources.TurbulentCatalyst, MetamorphResources.TurbulentCatalystEffect, RewardValue.Medium, "Turbulent_Catalyst.png"), // 5
            };
        }

        public List<MetamorphCatalyst> Catalysts { get; set; }
    }
}
