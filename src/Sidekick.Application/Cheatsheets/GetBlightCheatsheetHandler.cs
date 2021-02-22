using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Blight;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetBlightCheatsheetHandler : IQueryHandler<GetBlightCheatsheetQuery, BlightLeague>
    {
        private readonly BlightResources resources;

        public GetBlightCheatsheetHandler(
            BlightResources resources)
        {
            this.resources = resources;
        }

        public Task<BlightLeague> Handle(GetBlightCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var oils = new List<BlightOil>
            {
                new BlightOil(resources.GoldenOil, resources.GoldenOilEffect, RewardValue.High, "Golden_Oil.png"),
                new BlightOil(resources.SilverOil, resources.SilverOilEffect, RewardValue.High, "Silver_Oil.png"),
                new BlightOil(resources.OpalescentOil, resources.OpalescentOilEffect, RewardValue.Medium, "Opalescent_Oil.png"),
                new BlightOil(resources.BlackOil, resources.BlackOilEffect, RewardValue.Medium, "Black_Oil.png"),
                new BlightOil(resources.CrimsonOil, resources.CrimsonOilEffect, RewardValue.Medium, "Crimson_Oil.png"),
                new BlightOil(resources.VioletOil, resources.VioletOilEffect, RewardValue.Low, "Violet_Oil.png"),
                new BlightOil(resources.AzureOil, resources.AzureOilEffect, RewardValue.NoValue, "Azure_Oil.png"),
                new BlightOil(resources.TealOil, resources.TealOilEffect, RewardValue.Low, "Teal_Oil.png"),
                new BlightOil(resources.VerdantOil, resources.VerdantOilEffect, RewardValue.NoValue, "Verdant_Oil.png"),
                new BlightOil(resources.AmberOil, resources.AmberOilEffect, RewardValue.NoValue, "Amber_Oil.png"),
                new BlightOil(resources.SepiaOil, resources.SepiaOilEffect, RewardValue.NoValue, "Sepia_Oil.png"),
                new BlightOil(resources.ClearOil, resources.ClearOilEffect, RewardValue.NoValue, "Clear_Oil.png"),
            };

            return Task.FromResult(new BlightLeague()
            {
                Oils = oils
            });
        }
    }
}
