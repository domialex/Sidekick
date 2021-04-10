using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Heist;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetHeistCheatsheetHandler : IQueryHandler<GetHeistCheatsheetQuery, HeistLeague>
    {
        private readonly HeistResources resources;

        public GetHeistCheatsheetHandler(
            HeistResources resources)
        {
            this.resources = resources;
        }

        public Task<HeistLeague> Handle(GetHeistCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var jobs = new List<HeistJob>
            {
                new HeistJob(resources.Job_Lockpicking, "Lockpick.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Currency, RewardValue.High, "HeistRewardCurrency.png"),
                    new HeistReward(resources.Reward_Accessories, RewardValue.Medium, "HeistRewardTrinkets.png"),
                    new HeistReward(resources.Reward_Fragments, RewardValue.High, "HeistRewardFragments.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Karst, 5, "KarstIcon.png"),
                    new HeistAlly(resources.Ally_Kurai, 5, "KuraiIcon.png"),
                    new HeistAlly(resources.Ally_Huck, 3, "HuckIcon.png"),
                    new HeistAlly(resources.Ally_Tullina, 3, "TullinaIcon.png"),
                }),

                new HeistJob(resources.Job_BruteForce, "Bruteforce.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Uniques, RewardValue.Medium, "HeistRewardUniques.png"),
                    new HeistReward(resources.Reward_Weapon, RewardValue.NoValue, "HeistRewardWeapon.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Tibbs, 5, "TibbsIcon.png"),
                    new HeistAlly(resources.Ally_Huck, 3, "HuckIcon.png"),
                }),

                new HeistJob(resources.Job_Perception, "Perception.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Accessories, RewardValue.Medium, "HeistRewardTrinkets.png"),
                    new HeistReward(resources.Reward_Prophecies, RewardValue.Medium, "HeistRewardProphecies.png"),
                    new HeistReward(resources.Reward_DivinationCards, RewardValue.High, "HeistRewardDivination.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Adiyah, 5, "AdiyahIcon.png"),
                    new HeistAlly(resources.Ally_Nenet, 5, "NenetIcon.png"),
                    new HeistAlly(resources.Ally_Karst, 3, "KarstIcon.png"),
                    new HeistAlly(resources.Ally_Gianna, 2, "GiannaIcon.png"),
                }),

                new HeistJob(resources.Job_Demolition, "Demolition.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Generic, RewardValue.Low, "HeistRewardGeneric.png"),
                    new HeistReward(resources.Reward_Blight, RewardValue.High, "HeistRewardBlight.png"),
                    new HeistReward(resources.Reward_Metamorph, RewardValue.Medium, "HeistRewardMetamorph.png"),
                    new HeistReward(resources.Reward_Delirium, RewardValue.High, "HeistRewardDelirium.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Vinderi, 5, "VinderiIcon.png"),
                    new HeistAlly(resources.Ally_Tibbs, 4, "TibbsIcon.png"),
                    new HeistAlly(resources.Ally_Huck, 3, "HuckIcon.png"),
                }),

                new HeistJob(resources.Job_CounterThaumaturgy, "CounterThaumaturgy.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Gems, RewardValue.High, "HeistRewardGems.png"),
                    new HeistReward(resources.Reward_Accessories, RewardValue.Medium, "HeistRewardTrinkets.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Niles, 5, "NilesIcon.png"),
                    new HeistAlly(resources.Ally_Nenet, 4, "NenetIcon.png"),
                    new HeistAlly(resources.Ally_Gianna, 3, "GiannaIcon.png"),
                }),

                new HeistJob(resources.Job_TrapDisarmament, "TrapDisarmament.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Armour, RewardValue.NoValue, "HeistRewardArmour.png"),
                    new HeistReward(resources.Reward_Weapon, RewardValue.NoValue, "HeistRewardWeapon.png"),
                    new HeistReward(resources.Reward_Abyss, RewardValue.Low, "HeistRewardAbyss.png"),
                    new HeistReward(resources.Reward_Breach, RewardValue.Medium, "HeistRewardBreach.png"),
                    new HeistReward(resources.Reward_Talismans, RewardValue.NoValue, "HeistRewardTalismans.png"),
                    new HeistReward(resources.Reward_Legion, RewardValue.Medium, "HeistRewardLegion.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Vinderi, 5, "VinderiIcon.png"),
                    new HeistAlly(resources.Ally_Isla, 4, "IslaIcon.png"),
                    new HeistAlly(resources.Ally_Tullina, 2, "TullinaIcon.png"),
                }),

                new HeistJob(resources.Job_Agility, "Acrobatics.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Currency, RewardValue.High, "HeistRewardCurrency.png"),
                    new HeistReward(resources.Reward_Armour, RewardValue.NoValue, "HeistRewardArmour.png"),
                    new HeistReward(resources.Reward_Harbinger, RewardValue.Medium, "HeistRewardHarbinger.png"),
                    new HeistReward(resources.Reward_Essences, RewardValue.Low, "HeistRewardEssences.png"),
                    new HeistReward(resources.Reward_Fossils, RewardValue.High, "HeistRewardFossils.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Tullina, 5, "TullinaIcon.png"),
                    new HeistAlly(resources.Ally_Karst, 2, "KarstIcon.png"),
                }),

                new HeistJob(resources.Job_Deception, "Deception.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Armour, RewardValue.NoValue, "HeistRewardArmour.png"),
                    new HeistReward(resources.Reward_DivinationCards, RewardValue.High, "HeistRewardDivination.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Gianna, 5, "GiannaIcon.png"),
                    new HeistAlly(resources.Ally_Niles, 4, "NilesIcon.png"),
                }),

                new HeistJob(resources.Job_Engineering, "Engineering.png", new HeistReward[] {
                    new HeistReward(resources.Reward_Uniques, RewardValue.Medium, "HeistRewardUniques.png"),
                    new HeistReward(resources.Reward_Maps, RewardValue.High, "HeistRewardMaps.png"),
                    new HeistReward(resources.Reward_Armour, RewardValue.NoValue, "HeistRewardArmour.png"),
                }, new HeistAlly[] {
                    new HeistAlly(resources.Ally_Isla, 5, "IslaIcon.png"),
                    new HeistAlly(resources.Ally_Huck, 3, "HuckIcon.png"),
                    new HeistAlly(resources.Ally_Vinderi, 2, "VinderiIcon.png"),
                }),
            };

            return Task.FromResult(new HeistLeague()
            {
                Jobs = jobs
            });
        }
    }
}
