using System.Collections.Generic;
using Sidekick.Localization.Leagues.Blight;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Blight
{
    public class BlightLeague
    {
        public BlightLeague()
        {
            Oils = new List<BlightOil>()
            {
                new BlightOil(BlightResources.ClearOil, BlightResources.ClearOilEffect, RewardValue.NoValue, "Clear_Oil.png"),
                new BlightOil(BlightResources.SepiaOil, BlightResources.SepiaOilEffect, RewardValue.NoValue, "Sepia_Oil.png"),
                new BlightOil(BlightResources.AmberOil, BlightResources.AmberOilEffect, RewardValue.NoValue, "Amber_Oil.png"),
                new BlightOil(BlightResources.VerdantOil, BlightResources.VerdantOilEffect, RewardValue.NoValue, "Verdant_Oil.png"),
                new BlightOil(BlightResources.TealOil, BlightResources.TealOilEffect, RewardValue.Low, "Teal_Oil.png"),
                new BlightOil(BlightResources.AzureOil, BlightResources.AzureOilEffect, RewardValue.NoValue, "Azure_Oil.png"),
                new BlightOil(BlightResources.VioletOil, BlightResources.VioletOilEffect, RewardValue.Low, "Violet_Oil.png"),
                new BlightOil(BlightResources.CrimsonOil, BlightResources.CrimsonOilEffect, RewardValue.Medium, "Crimson_Oil.png"),
                new BlightOil(BlightResources.BlackOil, BlightResources.BlackOilEffect, RewardValue.Medium, "Black_Oil.png"),
                new BlightOil(BlightResources.OpalescentOil, BlightResources.OpalescentOilEffect, RewardValue.Medium, "Opalescent_Oil.png"),
                new BlightOil(BlightResources.SilverOil, BlightResources.SilverOilEffect, RewardValue.High, "Silver_Oil.png"),
                new BlightOil(BlightResources.GoldenOil, BlightResources.GoldenOilEffect, RewardValue.High, "Golden_Oil.png"),
            };
        }

        public List<BlightOil> Oils { get; set; }
    }
}
