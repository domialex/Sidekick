namespace Sidekick.Business.Languages.UI.Implementations
{
    public interface IUILanguage
    {
        string LanguageName { get; }

        #region Incursion
        string IncursionGuardhouse { get; }
        string IncursionBarracks { get; }
        string IncursionHallOfWar { get; }
        string IncursionGuardhouseModifiers { get; }
        string IncursionWorkshop { get; }
        string IncursionEngineeringDepartment { get; }
        string IncursionFactory { get; }
        string IncursionWorkshopContains { get; }
        string IncursionWorkshopModifiers { get; }
        string IncursionExplosivesRoom { get; }
        string IncursionDemolitionLab { get; }
        string IncursionShrineOfUnmaking { get; }
        string IncursionExplosivesRoomContains { get; }
        string IncursionSplinterResearchLab { get; }
        string IncursionBreachContainmentChamber { get; }
        string IncursionHouseOfOthers { get; }
        string IncursionSplinterResearchLabContains { get; }
        string IncursionVault { get; }
        string IncursionTreasury { get; }
        string IncursionWealthOfTheVaal { get; }
        string IncursionVaultContains { get; }
        string IncursionSparringRoom { get; }
        string IncursionArenaOfValour { get; }
        string IncursionHallOfChampions { get; }
        string IncursionSparringRoomContains { get; }
        string IncursionSparringRoomModifiers { get; }
        string IncursionArmourersWorkshop { get; }
        string IncursionArmoury { get; }
        string IncursionChamberOfIron { get; }
        string IncursionArmourersWorkshopContains { get; }
        string IncursionArmourersWorkshopModifiers { get; }
        string IncursionJewellersWorkshop { get; }
        string IncursionJewelleryForge { get; }
        string IncursionGlitteringHalls { get; }
        string IncursionJewellersWorkshopContains { get; }
        string IncursionSurveyorsStudy { get; }
        string IncursionOfficeOfCartography { get; }
        string IncursionAtlasOfWorlds { get; }
        string IncursionSurveyorsStudyContains { get; }
        string IncursionGemcuttersWorkshop { get; }
        string IncursionDepartmentOfThaumaturgy { get; }
        string IncursionDoryanisInstitute { get; }
        string IncursionGemcuttersWorkshopContains { get; }
        string IncursionTormentCells { get; }
        string IncursionTortureCages { get; }
        string IncursionSadistsDen { get; }
        string IncursionTormentCellsContains { get; }
        string IncursionStrongboxChamber { get; }
        string IncursionHallOfLocks { get; }
        string IncursionCourtOfTheSealedDeath { get; }
        string IncursionStrongboxChamberContains { get; }
        string IncursionHallOfMettle { get; }
        string IncursionHallOfHeroes { get; }
        string IncursionHallOfLegends { get; }
        string IncursionHallOfMettleContains { get; }
        string IncursionSacrificalChamber { get; }
        string IncursionHallOfOfferings { get; }
        string IncursionApexOfAscension { get; }
        string IncursionSacrificalChamberContains { get; }
        string IncursionStorageRoom { get; }
        string IncursionWarehouses { get; }
        string IncursionMuseumOfArtifacts { get; }
        string IncursionStorageRoomContains { get; }
        string IncursionCorruptionChamber { get; }
        string IncursionCatalystOfCorruption { get; }
        string IncursionLocuOfCorruption { get; }
        string IncursionCorruptionChamberContains { get; }
        string IncursionCorruptionChamberModifiers { get; }
        string IncursionShrineOfEmpowerment { get; }
        string IncursionSanctumOfUnity { get; }
        string IncursionTempleNexus { get; }
        string IncursionShrineOfEmpowermentContains { get; }
        string IncursionShrineOfEmpowermentModifiers { get; }
        string IncursionTempestGenerator { get; }
        string IncursionHurricaneEngine { get; }
        string IncursionStormOfCorruption { get; }
        string IncursionTempestGeneratorContains { get; }
        string IncursionTempestGeneratorModifiers { get; }
        string IncursionPoisionGarden { get; }
        string IncursionCultivarChamber { get; }
        string IncursionToxicGrove { get; }
        string IncursionPosionGardenContains { get; }
        string IncursionPoisonGardenModifiers { get; }
        string IncursionTrapWorkshop { get; }
        string IncursionTempleDefenseWorkshop { get; }
        string IncursionDefenseResearchLab { get; }
        string IncursionTrapWorkshopContains { get; }
        string IncursionTrapWorkshopModifiers { get; }
        string IncursionPoolsOfRestoration { get; }
        string IncursionSanctumOfVitality { get; }
        string IncursionSanctumOfImmortality { get; }
        string IncursionPoolsOfRestorationContains { get; }
        string IncursionPoolsOfRestorationModifiers { get; }
        string IncursionFlameWorkshop { get; }
        string IncursionOmnitectForge { get; }
        string IncursionCrucibleOfFlame { get; }
        string IncursionFlameWorkshopContains { get; }
        string IncursionFlameWorkshopModifiers { get; }
        string IncursionLightningWorkshop { get; }
        string IncursionOmnitectReactorPlant { get; }
        string IncursionConduitOfLightning { get; }
        string IncursionLightningWorkshopContains { get; }
        string IncursionLightningWorkshopModifiers { get; }
        string IncursionHatchery { get; }
        string IncursionAutomationLab { get; }
        string IncursionHybridisationChamber { get; }
        string IncursionHatcheryContains { get; }
        string IncursionHatcheryModifiers { get; }
        string IncursionRoyalMeetingRoom { get; }
        string IncursionHallOfLords { get; }
        string IncursionThroneOfAtziri { get; }
        string IncursionRoyalMeetingRoomContains { get; }
        string IncursionRoyalMeetingRoomModifiers { get; }

        string IncursionHeaderContains { get; }
        string IncursionHeaderModifiers { get; }

        string IncursionDoubleGemCorruptionTooltip { get; }
        string IncursionDoubleCorruptionTooltip { get; }
        string IncursionTopotanteModTooltip { get; }
        string IncursionTacatiModTooltip { get; }
        string IncursionMatalTooltip { get; }
        string IncursionGuateliztzModTooltip { get; }
        string IncursionPuhuarteModTooltuip { get; }
        string IncursionXopecModTooltip { get; }
        string IncursionCitaqualotlModTooltip { get; }

        #endregion

        #region Blight

        string BlightClearOil { get; }
        string BlightClearOilEffect { get; }
        string BlightSepiaOil { get; }
        string BlightSepiaOilEffect { get; }
        string BlightAmberOil { get; }
        string BlightAmberOilEffect { get; }
        string BlightVerdantOil { get; }
        string BlightVerdantOilEffect { get; }
        string BlightTealOil { get; }
        string BlightTealOilEffect { get; }
        string BlightAzureOil { get; }
        string BlightAzureOilEffect { get; }
        string BlightVioletOil { get; }
        string BlightVioletOilEffect { get; }
        string BlightCrimsonOil { get; }
        string BlightCrimsonOilEffect { get; }
        string BlightBlackOil { get; }
        string BlightBlackOilEffect { get; }
        string BlightOpalescentOil { get; }
        string BlightOpalescentOilEffect { get; }
        string BlightSilverOil { get; }
        string BlightSilverOilEffect { get; }
        string BlightGoldenOil { get; }
        string BlightGoldenOilEffect { get; }

        #endregion

        #region Metamorph

        string MetamorphAbrasiveCatalyst { get; }
        string MetamorphAbrasiveCatalystEffect { get; }
        string MetamorphFertileCatalyst { get; }
        string MetamorphFertileCatalystEffect { get; }
        string MetamorphImbuedCatalyst { get; }
        string MetamorphImbuedCatalystEffect { get; }
        string MetamorphIntrinsicCatalyst { get; }
        string MetamorphIntrinsicCatalystEffect { get; }
        string MetamorphPrismaticCatalyst { get; }
        string MetamorphPrismaticCatalystEffect { get; }
        string MetamorphTemperingCatalyst { get; }
        string MetamorphTemperingCatalystEffect { get; }
        string MetamorphTurbulentCatalyst { get; }
        string MetamorphTurbulentCatalystEffect { get; }
        string MetamorphInformationHeader { get; }
        string MetamorphInformationText { get; }

        #endregion

        #region Delve

        string DelveAberrantFossil { get; }
        string DelveAethericFossil { get; }
        string DelveBloodstainedFossil { get; }
        string DelveBoundFossil { get; }
        string DelveCorrodedFossil { get; }
        string DelveDenseFossil { get; }
        string DelveEnchantedFossil { get; }
        string DelveEncrustedFossil { get; }
        string DelveFacetedFossil { get; }
        string DelveFracturedFossil { get; }
        string DelveFrigidFossil { get; }
        string DelveGildedFossil { get; }
        string DelveGlyphicFossil { get; }
        string DelveHollowFossil { get; }
        string DelveJaggedFossil { get; }
        string DelveLucentFossil { get; }
        string DelveMetallicFossil { get; }
        string DelvePerfectFossil { get; }
        string DelvePrismaticFossil { get; }
        string DelvePristineFossil { get; }
        string DelveSanctifiedFossil { get; }
        string DelveScorchedFossil { get; }
        string DelveSerratedFossil { get; }
        string DelveShudderingFossil { get; }
        string DelveTangledFossil { get; }

        string DelveMines { get; }
        string DelveFungalCaverns { get; }
        string DelvePetrifiedForest { get; }
        string DelveAbyssalDepths { get; }
        string DelveFrozenHollow { get; }
        string DelveMagmaFissure { get; }
        string DelveSulfurVents { get; }
        string DelveFossilRoom { get; }
        string DelveInformation { get; }

        #endregion
    }
}
