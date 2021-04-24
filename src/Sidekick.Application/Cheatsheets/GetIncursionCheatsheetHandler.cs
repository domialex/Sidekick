using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Incursion;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetIncursionCheatsheetHandler : IQueryHandler<GetIncursionCheatsheetQuery, IncursionLeague>
    {
        private readonly IncursionResources resources;

        public GetIncursionCheatsheetHandler(
            IncursionResources resources)
        {
            this.resources = resources;
        }

        public Task<IncursionLeague> Handle(GetIncursionCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var rooms = new List<IncursionRoom>()
            {
                new IncursionRoom(string.Empty, resources.GuardhouseModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.Guardhouse, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.Barracks, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.HallOfWar, RewardValue.Low),
                },
                new IncursionRoom(resources.WorkshopContains, resources.WorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.Workshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.EngineeringDepartment, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.Factory, RewardValue.NoValue),
                },
                new IncursionRoom(resources.ExplosivesRoomContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.ExplosivesRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.DemolitionLab, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.ShrineOfUnmaking, RewardValue.Low),
                },
                new IncursionRoom(resources.SplinterResearchLabContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.SplinterResearchLab, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.BreachContainmentChamber, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(resources.HouseOfOthers, RewardValue.High),
                },
                new IncursionRoom(resources.VaultContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.Vault, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(resources.Treasury, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(resources.WealthOfTheVaal, RewardValue.High),
                },
                new IncursionRoom(resources.SparringRoomContains, resources.SparringRoomModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.SparringRoom, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.ArenaOfValour, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.HallOfChampions, RewardValue.NoValue),
                },
                new IncursionRoom(resources.ArmourersWorkshopContains, resources.ArmourersWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.ArmourersWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.Armoury, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.ChamberOfIron, RewardValue.NoValue),
                },
                new IncursionRoom(resources.JewellersWorkshopContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.JewellersWorkshop, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.JewelleryForge, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.GlitteringHalls, RewardValue.Low),
                },
                new IncursionRoom(resources.SurveyorsStudyContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.SurveyorsStudy, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(resources.OfficeOfCartography, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(resources.AtlasOfWorlds, RewardValue.Medium),
                },
                new IncursionRoom(resources.GemcuttersWorkshopContains, string.Empty)
                {
                    Tooltip = resources.DoubleGemCorruptionTooltip,
                    Level1 = new IncursionRoomTier(resources.GemcuttersWorkshop, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.DepartmentOfThaumaturgy, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.DoryanisInstitute, RewardValue.High),
                },
                new IncursionRoom(resources.TormentCellsContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.TormentCells, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.TortureCages, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.SadistsDen, RewardValue.NoValue),
                },
                new IncursionRoom(resources.StrongboxChamberContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.StrongboxChamber, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.HallOfLocks, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.CourtOfTheSealedDeath, RewardValue.Low),
                },
                new IncursionRoom(resources.HallOfMettleContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.HallOfMettle, RewardValue.Medium),
                    Level2 = new IncursionRoomTier(resources.HallOfHeroes, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(resources.HallOfLegends, RewardValue.High),
                },
                new IncursionRoom(resources.SacrificalChamberContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.SacrificialChamber, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.HallOfOfferings, RewardValue.Medium),
                    Level3 = new IncursionRoomTier(resources.ApexOfAscension, RewardValue.High),
                },
                new IncursionRoom(resources.StorageRoomContains, string.Empty)
                {
                    Level1 = new IncursionRoomTier(resources.StorageRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.Warehouses, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.MuseumOfArtifacts, RewardValue.Medium),
                },
                new IncursionRoom(resources.CorruptionChamberContains, resources.CorruptionChamberModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.CorruptionChamber, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.CatalystOfCorruption, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.LocusOfCorruption, RewardValue.High),
                },
                new IncursionRoom(resources.ShrineOfEmpowermentContains, resources.ShrineOfEmpowermentModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.ShrineOfEmpowerment, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.SanctumOfUnity, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.TempleNexus, RewardValue.High),
                },
                new IncursionRoom(resources.TempestGeneratorContains, resources.TempestGeneratorModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.TempestGenerator, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.HurricaneEngine, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.StormOfCorruption, RewardValue.Medium),
                },
                new IncursionRoom(resources.PoisonGardenContains, resources.PoisonGardenModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.PoisonGarden, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.CultivarChamber, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.ToxicGrove, RewardValue.Low),
                },
                new IncursionRoom(resources.TrapWorkshopContains, resources.TrapWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.TrapWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.TempleDefenseWorkshop, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.DefenseResearchLab, RewardValue.Low),
                },
                new IncursionRoom(resources.PoolsOfRestorationContains, resources.PoolsOfRestorationModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.PoolsOfRestoration, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.SanctumOfVitality, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.SanctumOfImmortality, RewardValue.Low),
                },
                new IncursionRoom(resources.FlameWorkshopContains, resources.FlameWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.FlameWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.OmnitectForge, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.CrucibleOfFlame, RewardValue.Low),
                },
                new IncursionRoom(resources.LightningWorkshopContains, resources.LightningWorkshopModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.LightningWorkshop, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.OmnitectReactorPlant, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.ConduitOfLightning, RewardValue.Low),
                },
                new IncursionRoom(resources.HatcheryContains, resources.HatcheryModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.Hatchery, RewardValue.NoValue),
                    Level2 = new IncursionRoomTier(resources.AutomatonLab, RewardValue.NoValue),
                    Level3 = new IncursionRoomTier(resources.HybridisationChamber, RewardValue.Low),
                },
                new IncursionRoom(resources.RoyalMeetingRoomContains, resources.RoyalMeetingRoomModifiers)
                {
                    Level1 = new IncursionRoomTier(resources.RoyalMeetingRoom, RewardValue.Low),
                    Level2 = new IncursionRoomTier(resources.HallOfLords, RewardValue.Low),
                    Level3 = new IncursionRoomTier(resources.ThroneOfAtziri, RewardValue.Medium),
                },
            };

            return Task.FromResult(new IncursionLeague()
            {
                Rooms = rooms
            });
        }
    }
}
