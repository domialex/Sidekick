using System.Collections.Generic;
using Sidekick.Localization.Leagues.Incursion;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Incursion
{
    public class IncursionLeague
    {
        public IncursionLeague()
        {
            Rooms = new List<IncursionRoom>()
            {
                new IncursionRoom(string.Empty, IncursionResources.GuardhouseModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.Guardhouse, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.Barracks, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.HallOfWar, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.WorkshopContains, IncursionResources.WorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.Workshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.EngineeringDepartment, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.Factory, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.ExplosivesRoomContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.ExplosivesRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.DemolitionLab, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.ShrineOfUnmaking, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.SplinterResearchLabContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.SplinterResearchLab, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.BreachContainmentChamber, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(IncursionResources.HouseOfOthers, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.VaultContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.Vault, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(IncursionResources.Treasury, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(IncursionResources.WealthOfTheVaal, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.SparringRoomContains, IncursionResources.SparringRoomModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.SparringRoom, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.ArenaOfValour, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.HallOfChampions, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.ArmourersWorkshopContains, IncursionResources.ArmourersWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.ArmourersWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.Armoury, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.ChamberOfIron, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.JewellersWorkshopContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.JewellersWorkshop, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.JewelleryForge, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.GlitteringHalls, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.SurveyorsStudyContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.SurveyorsStudy, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(IncursionResources.OfficeOfCartography, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(IncursionResources.AtlasOfWorlds, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.GemcuttersWorkshopContains, string.Empty)
                {
                    ContainsTooltip = IncursionResources.DoubleGemCorruptionTooltip,
                    Level1 = new IncursionRoomTier(IncursionResources.GemcuttersWorkshop, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.DepartmentOfThaumaturgy, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.DoryanisInstitute, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.TormentCellsContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.TormentCells, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.TortureCages, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.SadistsDen, RewardValue.NoValue),
                },
                new IncursionRoom(IncursionResources.StrongboxChamberContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.StrongboxChamber, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.HallOfLocks, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.CourtOfTheSealedDeath, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.HallOfMettleContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.HallOfMettle, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(IncursionResources.HallOfHeroes, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(IncursionResources.HallOfLegends, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.SacrificalChamberContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.SacrificialChamber, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.HallOfOfferings, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(IncursionResources.ApexOfAscension, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.StorageRoomContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.StorageRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.Warehouses, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.MuseumOfArtifacts, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.CorruptionChamberContains, IncursionResources.CorruptionChamberModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.CorruptionChamber, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.CatalystOfCorruption, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.LocusOfCorruption, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.ShrineOfEmpowermentContains, IncursionResources.ShrineOfEmpowermentModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.ShrineOfEmpowerment, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.SanctumOfUnity, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.TempleNexus, RewardValue.High),
                },
                new IncursionRoom(IncursionResources.TempestGeneratorContains, IncursionResources.TempestGeneratorModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.TempestGenerator, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.HurricaneEngine, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.StormOfCorruption, RewardValue.Medium),
                },
                new IncursionRoom(IncursionResources.PoisonGardenContains, IncursionResources.PoisonGardenModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.PoisonGarden, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.CultivarChamber, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.ToxicGrove, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.TrapWorkshopContains, IncursionResources.TrapWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.TrapWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.TempleDefenseWorkshop, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.DefenseResearchLab, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.PoolsOfRestorationContains, IncursionResources.PoolsOfRestorationModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.PoolsOfRestoration, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.SanctumOfVitality, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.SanctumOfImmortality, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.FlameWorkshopContains, IncursionResources.FlameWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.FlameWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.OmnitectForge, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.CrucibleOfFlame, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.LightningWorkshopContains, IncursionResources.LightningWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.LightningWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.OmnitectReactorPlant, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.ConduitOfLightning, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.HatcheryContains, IncursionResources.HatcheryModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.Hatchery, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(IncursionResources.AutomatonLab, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(IncursionResources.HybridisationChamber, RewardValue.Low),
                },
                new IncursionRoom(IncursionResources.RoyalMeetingRoomContains, IncursionResources.RoyalMeetingRoomModifiers)
                {
                    Level1 = new IncursionRoomTier(IncursionResources.RoyalMeetingRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(IncursionResources.HallOfLords, RewardValue.Low),
                    Level3 = new IncursionRoomTier(IncursionResources.ThroneOfAtziri, RewardValue.Medium),
                },
            };
        }

        public List<IncursionRoom> Rooms { get; set; }
    }
}
