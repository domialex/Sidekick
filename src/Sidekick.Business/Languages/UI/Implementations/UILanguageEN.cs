namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("en")]
    public class UILanguageEN : IUILanguage
    {
        public string LanguageName => "en";

        public string OverlayAccountName => "Account Name";
        public string OverlayCharacter => "Character";
        public string OverlayPrice => "Price";
        public string OverlayItemLevel => "iLvl";
        public string OverlayAge => "Age";

        public string LeagueLegendVeryValuable => "Very Valuable";
        public string LeagueLegendValuable => "Valuable";
        public string LeagueLegendLessValuable => "Less valuable";
        public string LeagueLegendNotValuable => "Not valuable";

        #region Betrayal
        public string BetrayalAislingTransportaion => "Veiled\nWeapons";
        public string BetrayalAislingFortification => "Veiled\nArmor";
        public string BetrayalAislingResearch => "Add Veiled\nMod(s)";
        public string BetrayalAislingIntervention => "Veiled\nJewellery";
        public string BetrayalCameriaTransportation => "Timeworn\nRelics";
        public string BetrayalCameriaFortification => "Harbinger\nOrbs";
        public string BetrayalCameriaResearch => "Sextants";
        public string BetrayalCameriaIntervention => "Sulphite\nScarab";
        public string BetrayalElreonTransportation => "Unique\nWeapons";
        public string BetrayalElreonFortification => "Unique\nArmor";
        public string BetrayalElreonResearch => "Unique\nJewellery";
        public string BetrayalElreonIntervention => "Reliquary\nScarabs";
        public string BetrayalGraviciusTransportation => "Div Card\nStack";
        public string BetrayalGraviciusFortification => "Random\nDiv Cards";
        public string BetrayalGraviciusResearch => "Swap\nDiv Cards";
        public string BetrayalGraviciusIntervention => "Div Card\nScarab";
        public string BetrayalGuffTransportation => "Chaos\nCrafting";
        public string BetrayalGuffFortification => "Essence\nCrafting";
        public string BetrayalGuffResearch => "Annull\\Ex\nCrafting";
        public string BetrayalGuffIntervention => "Alteration\nCrafting";
        public string BetrayalHakuTransportation => "Rare\nItems";
        public string BetrayalHakuFortification => "Strongboxes";
        public string BetrayalHakuResearch => "Quality\nItems";
        public string BetrayalHakuIntervention => "Strongbox\nScarab";
        public string BetrayalHillockTransportation => "Weapon\nQuality";
        public string BetrayalHillockFortification => "Armor\nQuality";
        public string BetrayalHillockResearch => "Flask\nQuality";
        public string BetrayalHillockIntervention => "Map\nQuality";
        public string BetrayalItThatFledTransportation => "Breach\nSplinter";
        public string BetrayalItThatFledFortification => "Abyss\nJewels";
        public string BetrayalItThatFledResearch => "Upgrade\nBreachstone";
        public string BetrayalItThatFledIntervention => "Breach\nScarab";
        public string BetrayalJanusTransportaion => "Quality\nCurrency";
        public string BetrayalJanusFortification => "Currency\nShards";
        public string BetrayalJanusResearch => "Perandus\nCoins";
        public string BetrayalJanusIntervention => "Perandus\nScarab";
        public string BetrayalJorginTransportation => "Talismans";
        public string BetrayalJorginFortification => "Bestiary\nItems";
        public string BetrayalJorginResearch => "Amulet to\nTalisman";
        public string BetrayalJorginIntervention => "Bestiary\nScarab";
        public string BetrayalKorrellTransportation => "Essences";
        public string BetrayalKorrelFortification => "Map\nFragments";
        public string BetrayalKorrellResearch => "Fossils";
        public string BetrayalKorrelIntervention => "Elder\nScarab";
        public string BetrayalLeoTransportation => "Silver\nCoins";
        public string BetrayalLeoFortification => "Random\nCurrency";
        public string BetrayalLeoResearch => "Currency\nUse";
        public string BetrayalLeoIntervention => "Torment\nScarab";
        public string BetrayalRikerTransportation => "Take One\nCurrency";
        public string BetrayalRikerFortification => "Take One\nUnique";
        public string BetrayalRikerResearch => "Take One\nVeiled";
        public string BetrayalRikerIntervention => "Take On\nDiv Card";
        public string BetrayalRinTransportation => "Normal\nMaps";
        public string BetrayalRinFortification => "Rare\nMaps";
        public string BetrayalRinResearch => "Unique\nMaps";
        public string BetrayalRinIntervention => "Cartography\nScarab";
        public string BetrayalToraTransportation => "Pick Item\n[Timed]";
        public string BetrayalToraFortification => "Enchanted\nItems";
        public string BetrayalToraResearch => "Gem\nExperience";
        public string BetrayalToraIntervention => "Harbinger\nScarab";
        public string BetrayalVaganTransportation => "Rare\nWeapons";
        public string BetrayalVaganFortification => "Rare\nArmor";
        public string BetrayalVaganResearch => "Rare\nJewellery";
        public string BetrayalVaganIntervention => "Rare\nJewels";
        public string BetrayalVoriciTransportation => "Quality\nGems";
        public string BetrayalVoriciFortification => "Linking\nCurrency";
        public string BetrayalVoriciResearch => "White\nSocktes";
        public string BetrayalVoriciIntervention => "Shaper\nScarab";

        public string BetrayalAislingResearchTooltip => "Level1: 1 Mod\nLevel2: Low Chance for 2 Mods\nLevel3: 2 Mods";
        public string BetrayalCameriaTransportationTooltip => "League Specific Items\n(Headhunter possible)";
        public string BetrayalGuffTransportationTooltip => "1 Alch\n200 Chaos\n6 Exalted\n20 Divine\n20 Blessed\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffFortificationTooltip => "20 Scouring\n5 * 4 Essences\n20 Exalted\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffResearchTooltip => "1 Alch\n100 Anullment\n100 Exalted\n1 Divine\n1 Blessed\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffInterventionTooltip => "1 Transmutation\n200 Alteration\n100 AUgmentation\n1 Regal\n1 Exalted\n1 Divine\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalHillockTransportationTooltip => "Level1: 24 %\nLevel2: 26 %\nLevel3: 28%";
        public string BetrayalHillockFortificationTooltip => "Level1: 24 %\nLevel2: 26 %\nLevel3: 28 %";
        public string BetrayalHillockResearchTooltip => "Level1: 22 %\nLevel2: 24 %\nLevel3: 26%";
        public string BetrayalHillockInterventionTooltip => "Level1: 25 %\nLevel2: 30 %\nLevel3: 35 %";
        public string BetrayalItThatFledResearchTooltip => "Level1: Charged\nLevel2: Enriched\nLevel3: Pure";
        public string BetrayalJorginResearchTooltip => "Level1: Level 1 Talisman\nLevel2: Level 2 Talisman\nLevel3: Level 3 Talisman";
        public string BetrayalLeoResearchTooltip => "Level1: Blessed Orb\nLevel2: Divine Orb\nLevel3: Exalted Orb";
        public string BetrayalToraTransportationTooltip => "Time Limit: 8 Seconds\nCurrency\nDivination Cards\nUniques\nItem Level 100 Rare";
        public string BetrayalToraFortificationTooltip => "Level1: Gloves\nLevel2: Boots\nLevel3: Helmet";
        public string BetrayalToraResearchTooltip => "Level1: 20m Exp\nLevel2: 70m Exp\nLevel3: 200m Exp";
        public string BetrayalVoriceResearchTooltip => "Level1: 1 White Socket\nLevel2: 1-2 White Sockets\nLevel3: 1-3 White Sockets";

        #endregion

        #region Incursion

        public string IncursionGuardhouse => "Guardhouse";
        public string IncursionBarracks => "Barracks";
        public string IncursionHallOfWar => "Hall of War";
        public string IncursionGuardhouseModifiers => "+10/20/30 % Monster Pack Size";
        public string IncursionWorkshop => "Workshop";
        public string IncursionEngineeringDepartment => "Engineering Department";
        public string IncursionFactory => "Factory";
        public string IncursionWorkshopContains => "Criticals Rank 3 Recipe";
        public string IncursionWorkshopModifiers => "5/10/15 % increased Quantity of Items\nUnique Boss has 15/25/35 % increased Life";
        public string IncursionExplosivesRoom => "Explosives Room";
        public string IncursionDemolitionLab => "Demolition Lab";
        public string IncursionShrineOfUnmaking => "Shrine of Unmaking";
        public string IncursionExplosivesRoomContains => "1/2/3 Explosive Kegs\n[Used to unlock doors]";
        public string IncursionSplinterResearchLab => "Splinter Research Lab";
        public string IncursionBreachContainmentChamber => "Breach Containment Chamber";
        public string IncursionHouseOfOthers => "House of the Others";
        public string IncursionSplinterResearchLabContains => "Level 1: 2 Clasped Hands\nLevel 2: 1 Breach\nLevel 3: 3 Breaches";
        public string IncursionVault => "Vault";
        public string IncursionTreasury => "Treasury";
        public string IncursionWealthOfTheVaal => "Wealth of the Vaal";
        public string IncursionVaultContains => "Currency Items inckuding Stacks and Shards";
        public string IncursionSparringRoom => "Sparring Room";
        public string IncursionArenaOfValour => "Arena of Valour";
        public string IncursionHallOfChampions => "Hall of Champions";
        public string IncursionSparringRoomContains => "     Weapons\nAccuracy Rank 3 Recipe";
        public string IncursionSparringRoomModifiers => "10/15/20 % increased Monster Damage";
        public string IncursionArmourersWorkshop => "Armourer's Workshop";
        public string IncursionArmoury => "Armoury";
        public string IncursionChamberOfIron => "Chamber of Iron";
        public string IncursionArmourersWorkshopContains => "     Armour\nAnimate Guardian Recipe";
        public string IncursionArmourersWorkshopModifiers => "10/20/30 & more Monster Life";
        public string IncursionJewellersWorkshop => "Jeweller's Workshop";
        public string IncursionJewelleryForge => "Jewellery Forge";
        public string IncursionGlitteringHalls => "Glittering Halls";
        public string IncursionJewellersWorkshopContains => "Jewellery, Talismans, Div Cards";
        public string IncursionSurveyorsStudy => "Surveyor's Study";
        public string IncursionOfficeOfCartography => "Office of Cartography";
        public string IncursionAtlasOfWorlds => "Atlas of Worlds";
        public string IncursionSurveyorsStudyContains => "Maps (possible Vaal Prefix)";
        public string IncursionGemcuttersWorkshop => "Gemcutter's Workshop";
        public string IncursionDepartmentOfThaumaturgy => "Department of Thaumaturgy";
        public string IncursionDoryanisInstitute => "Doryani's Institute";
        public string IncursionGemcuttersWorkshopContains => "     Gems\nLevel 3: Double Gem Corruption";
        public string IncursionTormentCells => "Torment Cells";
        public string IncursionTortureCages => "Torture Cages";
        public string IncursionSadistsDen => "Sadist's Den";
        public string IncursionTormentCellsContains => "Room contains 3/5/7 Tormented Spirits";
        public string IncursionStrongboxChamber => "Strongbox Chamber";
        public string IncursionHallOfLocks => "Hall of Locks";
        public string IncursionCourtOfTheSealedDeath => "Court of Sealed Death";
        public string IncursionStrongboxChamberContains => "Room contains 2/4/6 Stringboxes";
        public string IncursionHallOfMettle => "Hall of Mettle";
        public string IncursionHallOfHeroes => "Hall of Heroes";
        public string IncursionHallOfLegends => "Hall of Legends";
        public string IncursionHallOfMettleContains => "Level 1: Timeless Monolith\nLevel 2: Guaranteed War Hoard\nLevel 3: Guaranteed General";
        public string IncursionSacrificalChamber => "Sacrifical Chamber";
        public string IncursionHallOfOfferings => "Hall of Offerings";
        public string IncursionApexOfAscension => "Apex of Ascension";
        public string IncursionSacrificalChamberContains => "Level 1: Change Unique to other\nLevel 2: Can obtain League Specific Items\nLevel 3: Unique Item of same class/Vial Upgrade";
        public string IncursionStorageRoom => "Storage Room";
        public string IncursionWarehouses => "Warehouses";
        public string IncursionMuseumOfArtifacts => "Museum of Artifacts";
        public string IncursionStorageRoomContains => "Random Item-Specific Chests\nLevel 3: League Specific Items possible";
        public string IncursionCorruptionChamber => "Corruption Chamber";
        public string IncursionCatalystOfCorruption => "Catalyst of Corruption";
        public string IncursionLocuOfCorruption => "Locus of Corruption";
        public string IncursionCorruptionChamberContains => "Level 3: Double Corruption";
        public string IncursionCorruptionChamberModifiers => "-6/8/10 % maximum Player Resistances";
        public string IncursionShrineOfEmpowerment => "Shrine of Empowerment";
        public string IncursionSanctumOfUnity => "Sanctum of Unity";
        public string IncursionTempleNexus => "Temple Nexus";
        public string IncursionShrineOfEmpowermentContains => "Level 3: Upgrade connected rooms by 1 tier";
        public string IncursionShrineOfEmpowermentModifiers => "10/15/20 % increased Attack/Cast Speed";
        public string IncursionTempestGenerator => "Tempest Generator";
        public string IncursionHurricaneEngine => "Hurricane Engine";
        public string IncursionStormOfCorruption => "Storm of Corruption";
        public string IncursionTempestGeneratorContains => "Level 3: Topolante Mod";
        public string IncursionTempestGeneratorModifiers => "Adds Tempests";
        public string IncursionPoisionGarden => "Posion Garden";
        public string IncursionCultivarChamber => "Cultivar Chamber";
        public string IncursionToxicGrove => "Toxic Grove";
        public string IncursionPosionGardenContains => "Chaos Damage Rank 3 Recipe\nLevel 3: Tacati Mod or Apep's Slumber";
        public string IncursionPoisonGardenModifiers => "Spawn Caustic Ground Plants";
        public string IncursionTrapWorkshop => "Trap Workshop";
        public string IncursionTempleDefenseWorkshop => "Temple Defense Workshop";
        public string IncursionDefenseResearchLab => "Denfense Research Lab";
        public string IncursionTrapWorkshopContains => "Traps and Mines Rank 2 Recipe\nLevel 3: Matatl Mod or Architect's Hand";
        public string IncursionTrapWorkshopModifiers => "Adds Labyrinth Traps";
        public string IncursionPoolsOfRestoration => "Pools of Restoration";
        public string IncursionSanctumOfVitality => "Sanctum of Vitality";
        public string IncursionSanctumOfImmortality => "Sanctum of Immortality";
        public string IncursionPoolsOfRestorationContains => "     Leech Rank 2 Recipe\nLevel 3: Guatelitzi Mod or Mask of the Spirit Drinker";
        public string IncursionPoolsOfRestorationModifiers => "Monster regenerate 4/6/8 % Life per Second";
        public string IncursionFlameWorkshop => "Flame Workshop";
        public string IncursionOmnitectForge => "Omnitect Forge";
        public string IncursionCrucibleOfFlame => "Crucible of Flame";
        public string IncursionFlameWorkshopContains => "Level 3: Puhuarte Mod or Story of the Vaal";
        public string IncursionFlameWorkshopModifiers => "Augments the Omnitect with fire";
        public string IncursionLightningWorkshop => "Lightning Workshop";
        public string IncursionOmnitectReactorPlant => "Omnitect Reactor Plant";
        public string IncursionConduitOfLightning => "Conduit of Lightning";
        public string IncursionLightningWorkshopContains => "Level 3: Xopec Mod or Dance o fthe Offered";
        public string IncursionLightningWorkshopModifiers => "Augments the Omnitect with lightning";
        public string IncursionHatchery => "Hatchery";
        public string IncursionAutomationLab => "Automation Lab";
        public string IncursionHybridisationChamber => "Hybridisation Chamber";
        public string IncursionHatcheryContains => "Level 3: Citaqualotl Mod or Coward's Chains";
        public string IncursionHatcheryModifiers => "Augments the Omnitect with minions";
        public string IncursionRoyalMeetingRoom => "Royal Meeting Room";
        public string IncursionHallOfLords => "Hall of Lords";
        public string IncursionThroneOfAtziri => "Throne of Atziri";
        public string IncursionRoyalMeetingRoomContains => "Level 3: Atziri Boss";
        public string IncursionRoyalMeetingRoomModifiers => "10/15/20 % increased Attack/Cast Speed";

        public string IncursionHeaderContains => "Contains";
        public string IncursionHeaderModifiers => "Modifiers";

        public string IncursionDoubleGemCorruptionTooltip => "Applies to corruptions to a Gem\nPossible to get a 21/23 Gem";
        public string IncursionDoubleCorruptionTooltip => "Possible outcomes: \nChange all sockets to white \nAdd two imnplicit Vaal modifiers \nTransform Sacrifical Garb into Shadowstitch \nTransform into a rare Item with random influence \nDestroy the item";
        public string IncursionTopotanteModTooltip => "Possible Prefixes: \nAdd Fire/Cold/Lightning Damage to Attacks\n25% of Physical Damage Converted to Fire/Cold/Lighning Damage\n" +
                                                    "---------------------------------------------------------------------\nAdd Fire/Cold/Lightning Damage \nDamage Penetrates 13-15% Fire/Cold/Lightning Resistance \n" +
                                                    "---------------------------------------------------------------------\n75-115% increased Fire/Cold/Lightning Damage \nAdd Fire/Cold/Lightning Damage to Spells";
        public string IncursionTacatiModTooltip => "Prefix: \n" +
                                                   "155-169% increased Physical Damage+\n" +
                                                   "Gain 9-10% of Physical Damage as Extra Chaos Damage\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "70-110% increased Spell Damage\n" +
                                                   "Gain 5% of Non-Chaos Damage as Extra Chaos Damage\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "Suffix: \n" +
                                                   "23-38% increased Cast Speed\n" +
                                                   "Adds 17-57 Chaos Damage to Spells\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "14-27% increased Attack Speed\n" +
                                                   "Adds 23-61 Chaos Damage\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "31-35% to Chaos Resistance\n" +
                                                   "9-10% reduced Chaos Damage taken Over Time\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "13-18% increased Posision Duration\n" +
                                                   "26-30% increased Chaos Damage\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "30% chance to Poision on Hit\n" +
                                                   "26-30% increased Chaos Damage\n" +
                                                   "--------------------------------------------------------\n" +
                                                   "31-35% increased Damage with Poison\n" +
                                                   "Adds 17-61 Chaos Damage";

        public string IncursionMatalTooltip => "Prefix: \n" +
                                               "30% increased Movement Speed\n" +
                                               "5-6% chance to Dodge Spell Hits\n" +
                                               "--------------------------------------\n" +
                                               "90-138% increased Trap/Mine Damage\n" +
                                               "--------------------------------------\n" +
                                               "+2 to Level of socketed Trap/Trap or Mine Gems\n" +
                                               "--------------------------------------\n" +
                                               "Suffix: \n" +
                                               "20-33% increased Trap/Mine Throwing Speed\n" +
                                               "--------------------------------------\n" +
                                               "14-22% increased Cooldown Recovery Speed for Throwing Traps\n" +
                                               "17-30% increased Trap Duration\n" +
                                               "--------------------------------------\n" +
                                               "Mines have 14-22% increased Detonation Speed\n" +
                                               "17-30% increased Mine Duration\n" +
                                               "--------------------------------------\n" +
                                               "Skills used by Traps/Mines have 22-37% increased Area of Effect";
        public string IncursionGuateliztzModTooltip => "Prefix: \n" +
                                                       "+70-119 to Maximum Life\n" +
                                                       "3-10% increased Maximum Life+\n" +
                                                       "--------------------------------\n" +
                                                       "+44-47 to Maximum Energy Shield\n" +
                                                       "7-10% increased Maximum Energy Shield\n" +
                                                       "--------------------------------\n" +
                                                       "+44-47 to Maximum Energy Shield\n" +
                                                       "Regenerate 0.4% of Energy Shield per Second\n" +
                                                       "--------------------------------\n" +
                                                       "Suffix: \n" +
                                                       "Regenerate 16-20 Life per Second\n" +
                                                       "Regenerate 0.4% of Life per Second";
        public string IncursionPuhuarteModTooltuip => "Suffix: \n" +
                                                      "+46-48% to Fire/Cold/Lightning Resistance\n" +
                                                      "9-10% of Physical Damage from Hits taken as Fire/Cold/Lightning Damage\n" +
                                                      "------------------------------------------\n" +
                                                      "+46-48% to Fire/Cold/Lightning Resistance\n" +
                                                      "0.4% of Fire/Cold/Lightning Damage leeched as Life\n" +
                                                      "------------------------------------------\n" +
                                                      "+46-48% to Fire/Cold/Lightning Resistance\n" +
                                                      "On of the following matching the Resistance:\n" +
                                                      "(45-52) to (75-78) added Fire Damage against Burning Enemies\n" +
                                                      "(30-50)% increased Damage with Hits against Chilled Enemies\n" +
                                                      "(40-60)% increased Critical Strike Chance against Shocked Enemies";
        public string IncursionXopecModTooltip => "Prefix: \n" +
                                                  "+69-73 to maximum Mana\n" +
                                                  "7-10% increased maximum Mana\n" +
                                                  "--------------------------------\n" +
                                                  "+74-78 to maximum Mana\n" +
                                                  "+2-3 Mana gained for each Enemy hit by your Attacks\n" +
                                                  "--------------------------------\n" +
                                                  "+74-78 to maximum Mana\n" +
                                                  "Regenerate 5-7 Mana per Second\n" +
                                                  "--------------------------------\n" +
                                                  "+69-73 to maximum Mana\n" +
                                                  "3-5% reduced Mana Reserved\n" +
                                                  "--------------------------------\n" +
                                                  "+74-78 to maximum Mana\n" +
                                                  "-6-8 to Total Mana Cost of Skills\n" +
                                                  "--------------------------------\n" +
                                                  "+74-78 to maximum Mana\n" +
                                                  "Non-Channelling Skills have -6-8 to Total Mana Cost";
        public string IncursionCitaqualotlModTooltip => "Prefix: \n" +
                                                        "Minions deal 90-138 increased Damage\n" +
                                                        "------------------------------------\n" +
                                                        "+2 to Level of socketed Minion Gems\n" +
                                                        "------------------------------------\n" +
                                                        "Minions deal 70-110 increased Damage\n" +
                                                        "Minions have 5% Chance to deal Double Damage\n" +
                                                        "------------------------------------\n" +
                                                        "Suffix: \n" +
                                                        "Minions have 13-40% increased Attack Speed\n" +
                                                        "Minions have 13-40% increased Cast Speed\n" +
                                                        "------------------------------------\n" +
                                                        "17-30% increased Minion Duration";

        #endregion

        #region Blight

        public string BlightClearOil => "Clear Oil";
        public string BlightClearOilEffect => "+5% Monster Pack Size\n" +
                                              "+10% reduced Monster Movement Speed";
        public string BlightSepiaOil => "Sepia Oil";
        public string BlightSepiaOilEffect => "+5% Monster Pack Size\n" +
                                              "Towers deal 20% more Damage";
        public string BlightAmberOil => "Amber Oil";
        public string BlightAmberOilEffect => "+5% Monster Pack Size\n" +
                                              "20% reduced Cost of Towers";
        public string BlightVerdantOil => "Verdant Oil";
        public string BlightVerdantOilEffect => "+15% Monster Pack Size";
        public string BlightTealOil => "Teal Oil";
        public string BlightTealOilEffect => "+5% Monster Pack Size\n" +
                                             "2 Blight Chests are Lucky";
        public string BlightAzureOil => "Azure Oil";
        public string BlightAzureOilEffect => "+5% Monster Pack Size\n" +
                                              "15% increased Experience";
        public string BlightVioletOil => "Violet Oil";
        public string BlightVioletOilEffect => "+5% Monster Pack Size\n" +
                                               "30% increased Quantity of Items";
        public string BlightCrimsonOil => "Crimson Oil";
        public string BlightCrimsonOilEffect => "+5% Monster Pack Size\n" +
                                                "3 Blight Chests are Lucky";
        public string BlightBlackOil => "Black Oil";
        public string BlightBlackOilEffect => "+5% Monster Pack Size\n" +
                                              "10% chance for Blight Chests to have an\n" +
                                              "additional reward";
        public string BlightOpalescentOil => "Opalescent Oil";
        public string BlightOpalescentOilEffect => "+25% Monster Pack Size";
        public string BlightSilverOil => "Silver Oil";
        public string BlightSilverOilEffect => "+5% Monster Pack Size\n" +
                                               "5 Blight Chests are Lucky";
        public string BlightGoldenOil => "Golden Oil";
        public string BlightGoldenOilEffect => "+5% Monster Pack Size\n" +
                                               "25% chance for Blight Chests to have an\n" +
                                               "additional Reward";

        #endregion

        #region Metamorph

        public string MetamorphAbrasiveCatalyst => "Abrasive Catalyst";

        public string MetamorphAbrasiveCatalystEffect => "Add Quality that enchances\n" +
                                                         "Attack modifiers";

        public string MetamorphFertileCatalyst => "Fertile Catalyst";

        public string MetamorphFertileCatalystEffect => "Add Quality that enhances\n" +
                                                        "Life and Mana Modifiers";

        public string MetamorphImbuedCatalyst => "Imbued Catalyst";

        public string MetamorphImbuedCatalystEffect => "Add Quality that enhances\n" +
                                                       "Caster Modifiers";

        public string MetamorphIntrinsicCatalyst => "Intrinsic Catalyst";

        public string MetamorphIntrinsicCatalystEffect => "Adds Quality that enhances\n" +
                                                          "Attribute Modifiers";

        public string MetamorphPrismaticCatalyst => "Prismatic Catalyst";

        public string MetamorphPrismaticCatalystEffect => "Adds Quality that enhances\n" +
                                                          "Resistance Modifiers";

        public string MetamorphTemperingCatalyst => "Tempering Catalyst";

        public string MetamorphTemperingCatalystEffect => "Add Quality that enhances\n" +
                                                          "Defense Modifiers";

        public string MetamorphTurbulentCatalyst => "Turbulent Catalyst";

        public string MetamorphTurbulentCatalystEffect => "Adds Quality that enhances\n" +
                                                          "Elemental Damage Modifiers";

        public string MetamorphInformationHeader => "Information";

        public string MetamorphInformationText => "Can be applied to: \n" +
                                                  "Rings, Amulets, Belts \n" +
                                                  "Applying a Catalyst removes all other Quality";

        #endregion

        #region Delve

        public string DelveAberrantFossil => "Abberant Fossil";
        public string DelveAethericFossil => "Aetheric Fossil";
        public string DelveBloodstainedFossil => "Bloodstained Fossil";
        public string DelveBoundFossil => "Bound Fossil";
        public string DelveCorrodedFossil => "Corroded Fossil";
        public string DelveDenseFossil => "Dense Fossil";
        public string DelveEnchantedFossil => "Enchanted Fossil";
        public string DelveEncrustedFossil => "Encrusted Fossil";
        public string DelveFacetedFossil => "Faceted Fossil";
        public string DelveFracturedFossil => "Fractured Fossil";
        public string DelveFrigidFossil => "Frigid Fossil";
        public string DelveGildedFossil => "Gilded Fossil";
        public string DelveGlyphicFossil => "Glyphic Fossil";
        public string DelveHollowFossil => "Hollow Fossil";
        public string DelveJaggedFossil => "Jagged Fossil";
        public string DelveLucentFossil => "Lucent Fossil";
        public string DelveMetallicFossil => "Metallic Fossil";
        public string DelvePerfectFossil => "Perfect Fossil";
        public string DelvePrismaticFossil => "Prismatic Fossil";
        public string DelvePristineFossil => "Pristine Fossil";
        public string DelveSanctifiedFossil => "Sanctified Fossil";
        public string DelveScorchedFossil => "Scorched Fossil";
        public string DelveSerratedFossil => "Serrated Fossil";
        public string DelveShudderingFossil => "Shuddering Fossil";
        public string DelveTangledFossil => "Tangled Fossil";
        public string DelveMines => "Mines";
        public string DelveFungalCaverns => "Fungal Caverns";
        public string DelvePetrifiedForest => "Petrified Forest";
        public string DelveAbyssalDepths => "Abyssal Depths";
        public string DelveFrozenHollow => "Frozen Hollow";
        public string DelveMagmaFissure => "Magma Fissure";
        public string DelveSulfurVents => "Sulfur Vents";
        public string DelveFossilRoom => "Fossil Room";
        public string DelveInformation => "*Can only be found behind fractured walls";

        #endregion
    }
}
