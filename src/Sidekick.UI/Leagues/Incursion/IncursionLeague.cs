using System.Collections.Generic;
using Sidekick.Localization.Leagues.Incursion;

namespace Sidekick.UI.Leagues.Incursion
{
    public class IncursionLeague
    {
        public IncursionLeague()
        {
            Rooms = new List<IncursionRoom>()
            {
                new IncursionRoom(string.Empty, IncursionResources.GuardhouseModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.Guardhouse, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.Barracks, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.HallOfWar, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.WorkshopContains, IncursionResources.WorkshopModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.Workshop, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.EngineeringDepartment, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.Factory, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.ExplosivesRoomContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.ExplosivesRoom, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.DemolitionLab, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.ShrineOfUnmaking, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.SplinterResearchLabContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.SplinterResearchLab, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.BreachContainmentChamber, RewardValue.Medium),
                    Tier3 = new IncursionRoomTier(IncursionResources.HouseOfOthers, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.VaultContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.Vault, RewardValue.Medium),
                    Tier2 = new IncursionRoomTier(IncursionResources.Treasury, RewardValue.Medium),
                    Tier3 = new IncursionRoomTier(IncursionResources.WealthOfTheVaal, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.SparringRoomContains, IncursionResources.SparringRoomModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.SparringRoom, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.ArenaOfValour, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.HallOfChampions, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.ArmourersWorkshopContains, IncursionResources.ArmourersWorkshopModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.ArmourersWorkshop, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.Armoury, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.ChamberOfIron, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.JewellersWorkshopContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.JewellersWorkshop, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.JewelleryForge, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.GlitteringHalls, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.SurveyorsStudyContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.SurveyorsStudy, RewardValue.Medium),
                    Tier2 = new IncursionRoomTier(IncursionResources.OfficeOfCartography, RewardValue.Medium),
                    Tier3 = new IncursionRoomTier(IncursionResources.AtlasOfWorlds, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.GemcuttersWorkshopContains, string.Empty)
                {
                    ContainsTooltip = IncursionResources.DoubleGemCorruptionTooltip,
                    Tier1 = new IncursionRoomTier(IncursionResources.GemcuttersWorkshop, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.DepartmentOfThaumaturgy, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.DoryanisInstitute, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.TormentCellsContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.TormentCells, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.TortureCages, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.SadistsDen, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.StrongboxChamberContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.StrongboxChamber, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.HallOfLocks, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.CourtOfTheSealedDeath, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.HallOfMettleContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.HallOfMettle, RewardValue.Medium),
                    Tier2 = new IncursionRoomTier(IncursionResources.HallOfHeroes, RewardValue.Medium),
                    Tier3 = new IncursionRoomTier(IncursionResources.HallOfLegends, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.SacrificalChamberContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.SacrificialChamber, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.HallOfOfferings, RewardValue.Medium),
                    Tier3 = new IncursionRoomTier(IncursionResources.ApexOfAscension, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.StorageRoomContains, string.Empty)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.StorageRoom, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.Warehouses, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.MuseumOfArtifacts, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.CorruptionChamberContains, IncursionResources.CorruptionChamberModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.CorruptionChamber, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.CatalystOfCorruption, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.LocusOfCorruption, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.ShrineOfEmpowermentContains, IncursionResources.ShrineOfEmpowermentModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.ShrineOfEmpowerment, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.SanctumOfUnity, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.TempleNexus, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.TempestGeneratorContains, IncursionResources.TempestGeneratorModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.TempestGenerator, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.HurricaneEngine, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.StormOfCorruption, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.PoisonGardenContains, IncursionResources.PoisonGardenModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.PoisonGarden, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.CultivarChamber, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.ToxicGrove, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.TrapWorkshopContains, IncursionResources.TrapWorkshopModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.TrapWorkshop, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.TempleDefenseWorkshop, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.DefenseResearchLab, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.PoolsOfRestorationContains, IncursionResources.PoolsOfRestorationModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.PoolsOfRestoration, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.SanctumOfVitality, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.SanctumOfImmortality, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.FlameWorkshopContains, IncursionResources.FlameWorkshopModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.FlameWorkshop, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.OmnitectForge, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.CrucibleOfFlame, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.LightningWorkshopContains, IncursionResources.LightningWorkshopModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.LightningWorkshop, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.OmnitectReactorPlant, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.ConduitOfLightning, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.HatcheryContains, IncursionResources.HatcheryModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.Hatchery, RewardValue.NoValue),
                    Tier2 = new IncursionRoomTier(IncursionResources.AutomatonLab, RewardValue.NoValue),
                    Tier3 = new IncursionRoomTier(IncursionResources.HybridisationChamber, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.RoyalMeetingRoomContains, IncursionResources.RoyalMeetingRoomModifiers)
                {
                    Tier1 = new IncursionRoomTier(IncursionResources.RoyalMeetingRoom, RewardValue.Low),
                    Tier2 = new IncursionRoomTier(IncursionResources.HallOfLords, RewardValue.Low),
                    Tier3 = new IncursionRoomTier(IncursionResources.ThroneOfAtziri, RewardValue.Medium),
                },
            };
        }

        public List<IncursionRoom> Rooms { get; set; }
    }
}
