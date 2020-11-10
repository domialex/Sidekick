using System.Collections.Generic;
using Sidekick.Localization.Leagues.Heist;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Heist
{
    public class HeistLeague
    {
        public HeistLeague()
        {
            //Agents = new List<BetrayalAgent>();
            Jobs = new List<HeistJob>();

            Jobs.Add(new HeistJob("Lockpicking", "Lockpick.png", new HeistReward[] {
                new HeistReward("Currency", RewardValue.High, "HeistRewardCurrency.png"),
                new HeistReward("Accessories", RewardValue.Medium, "HeistRewardTrinkets.png"),
                new HeistReward("Fragments", RewardValue.High, "HeistRewardFragments.png"),
            }, new HeistAlly[] {
                new HeistAlly("Karst", 5, "KarstIcon.png"),
                new HeistAlly("Kurai", 5, "KuraiIcon.png"),
                new HeistAlly("Huck", 3, "HuckIcon.png"),
                new HeistAlly("Tullina", 3, "TullinaIcon.png"),
            }));

            Jobs.Add(new HeistJob("Brute Force", "Bruteforce.png", new HeistReward[] {
                new HeistReward("Uniques", RewardValue.Medium, "HeistRewardUniques.png"),
                new HeistReward("Weapon", RewardValue.NoValue, "HeistRewardWeapon.png"),
            }, new HeistAlly[] {
                new HeistAlly("Tibbs", 5, "TibbsIcon.png"),
                new HeistAlly("Huck", 3, "HuckIcon.png"),
            }));

            Jobs.Add(new HeistJob("Perception", "Perception.png", new HeistReward[] {
                new HeistReward("Accessories", RewardValue.Medium, "HeistRewardTrinkets.png"),
                new HeistReward("Prophecies", RewardValue.Medium, "HeistRewardProphecies.png"),
                new HeistReward("Divination Cards", RewardValue.High, "HeistRewardDivination.png"),
            }, new HeistAlly[] {
                new HeistAlly("Adiyah", 5, "AdiyahIcon.png"),
                new HeistAlly("Nenet", 5, "NenetIcon.png"),
                new HeistAlly("Karst", 3, "KarstIcon.png"),
                new HeistAlly("Gianna", 2, "GiannaIcon.png"),
            }));

            Jobs.Add(new HeistJob("Demolition", "Demolition.png", new HeistReward[] {
                new HeistReward("Generic", RewardValue.Low, "HeistRewardGeneric.png"),
                new HeistReward("Blight", RewardValue.High, "HeistRewardBlight.png"),
                new HeistReward("Metamorph", RewardValue.Medium, "HeistRewardMetamorph.png"),
                new HeistReward("Delirium", RewardValue.High, "HeistRewardDelirium.png"),
            }, new HeistAlly[] {
                new HeistAlly("Vinderi", 5, "VinderiIcon.png"),
                new HeistAlly("Tibbs", 4, "TibbsIcon.png"),
                new HeistAlly("Huck", 3, "HuckIcon.png"),
            }));

            Jobs.Add(new HeistJob("Counter-Thaumaturgy", "CounterThaumaturgy.png", new HeistReward[] {
                new HeistReward("Gems", RewardValue.High, "HeistRewardGems.png"),
                new HeistReward("Accessories", RewardValue.Medium, "HeistRewardTrinkets.png"),
            }, new HeistAlly[] {
                new HeistAlly("Niles", 5, "NilesIcon.png"),
                new HeistAlly("Nenet", 4, "NenetIcon.png"),
                new HeistAlly("Gianna", 3, "GiannaIcon.png"),
            }));

            Jobs.Add(new HeistJob("Trap Disarmament", "TrapDisarmament.png", new HeistReward[] {
                new HeistReward("Armour", RewardValue.NoValue, "HeistRewardArmour.png"),
                new HeistReward("Weapon", RewardValue.NoValue, "HeistRewardWeapon.png"),
                new HeistReward("Abyss", RewardValue.Low, "HeistRewardAbyss.png"),
                new HeistReward("Breach", RewardValue.Medium, "HeistRewardBreach.png"),
                new HeistReward("Talismans", RewardValue.NoValue, "HeistRewardTalismans.png"),
                new HeistReward("Legion", RewardValue.Medium, "HeistRewardLegion.png"),
            }, new HeistAlly[] {
                new HeistAlly("Vinderi", 5, "VinderiIcon.png"),
                new HeistAlly("Isla", 4, "IslaIcon.png"),
                new HeistAlly("Tullina", 2, "TullinaIcon.png"),
            }));

            Jobs.Add(new HeistJob("Agility", "Acrobatics.png", new HeistReward[] {
                new HeistReward("Currency", RewardValue.High, "HeistRewardCurrency.png"),
                new HeistReward("Armour", RewardValue.NoValue, "HeistRewardArmour.png"),
                new HeistReward("Harbinger", RewardValue.Medium, "HeistRewardHarbinger.png"),
                new HeistReward("Essences", RewardValue.Low, "HeistRewardEssences.png"),
                new HeistReward("Fossils", RewardValue.High, "HeistRewardFossils.png"),
            }, new HeistAlly[] {
                new HeistAlly("Tullina", 5, "TullinaIcon.png"),
                new HeistAlly("Karst", 2, "KarstIcon.png"),
            }));

            Jobs.Add(new HeistJob("Deception", "Deception.png", new HeistReward[] {
                new HeistReward("Armour", RewardValue.NoValue, "HeistRewardArmour.png"),
                new HeistReward("Divination Cards", RewardValue.High, "HeistRewardDivination.png"),
            }, new HeistAlly[] {
                new HeistAlly("Gianna", 5, "GiannaIcon.png"),
                new HeistAlly("Niles", 4, "NilesIcon.png"),
            }));

            Jobs.Add(new HeistJob("Engineering", "Engineering.png", new HeistReward[] {
                new HeistReward("Uniques", RewardValue.Medium, "HeistRewardUniques.png"),
                new HeistReward("Maps", RewardValue.High, "HeistRewardMaps.png"),
                new HeistReward("Armour", RewardValue.NoValue, "HeistRewardArmour.png"),
            }, new HeistAlly[] {
                new HeistAlly("Isla", 5, "IslaIcon.png"),
                new HeistAlly("Huck", 3, "HuckIcon.png"),
                new HeistAlly("Vinderi", 2, "VinderiIcon.png"),
            }));
        }

        public List<HeistJob> Jobs { get; private set; }
    }
}
