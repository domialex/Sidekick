using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Sidekick.Localization.Leagues;
using Sidekick.Localization.Leagues.Betrayal;
using Sidekick.Localization.Leagues.Blight;
using Sidekick.Localization.Leagues.Delve;
using Sidekick.Localization.Leagues.Incursion;
using Sidekick.Localization.Leagues.Metamorph;

namespace Sidekick.Windows.LeagueOverlay
{
    /// <summary>
    /// Interaction logic for LeagueOverlayView.xaml
    /// </summary>
    public partial class LeagueOverlayView : Window
    {
        private Dictionary<TabItem, int[]> tabPageSizeDictionary;
        private TabItem CurrentPage;
        private Dictionary<string, string> DelveFossilRarityDictionary;

        public const string VeryLowValueColorName = "VeryLowValueColor";
        public const string LowValueColorName = "LowValueColor";
        public const string MediumValueColorName = "MediumValueColor";
        public const string HighValueColorName = "HighValueColor";

        public LeagueOverlayView()
        {
            InitializeComponent();

            UpdateFossilRarityDictionary();
            UpdateHeaderUIText();
            UpdateBetrayalUIText();
            UpdateIncursionUIText();
            UpdateBlightUIText();
            UpdateMetamorphUIText();
            UpdateDelveUIText();

            Legacy.UILanguageProvider.UILanguageChanged += UpdateFossilRarityDictionary;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateHeaderUIText;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateBetrayalUIText;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateIncursionUIText;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateBlightUIText;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateMetamorphUIText;
            Legacy.UILanguageProvider.UILanguageChanged += UpdateDelveUIText;

            tabPageSizeDictionary = new Dictionary<TabItem, int[]>()
            {
                { tabItemIncursion, new[] { 980, 1050 } },
                { tabItemDelve, new[] { 425, 1015 } },
                { tabItemBetrayal, new[] { 520, 1200 } },
                { tabItemBlight, new[] { 605, 1165 } },
                { tabItemMetamorph, new[] { 315, 1115 } },
            };

            tabControlLeagueOverlay.SelectionChanged += TabControlLeagueOverlay_SelectionChanged;
            tabControlLeagueOverlay.MouseWheel += TabControlLeagueOverlay_MouseWheel;
            CurrentPage = tabItemIncursion;
        }

        public bool IsDisplayed => Visibility == Visibility.Visible;

        private void TabControlLeagueOverlay_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var currentPageIndex = tabControlLeagueOverlay.SelectedIndex;

            if (e.Delta > 0 && currentPageIndex > 0)       // Scroll Up
            {
                tabControlLeagueOverlay.SelectedIndex = --currentPageIndex;
            }
            else if (e.Delta < 0 && currentPageIndex <= tabControlLeagueOverlay.Items.Count)                   // Scroll Down
            {
                tabControlLeagueOverlay.SelectedIndex = ++currentPageIndex;
            }
        }

        public void ShowWindow()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ShowWindowCallback(ShowWindow));
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }
        delegate void ShowWindowCallback();

        public void HideWindow()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HideWindowCallback(HideWindow));
            }
            else
            {
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowCallback();

        public void SetWindowPosition(int x, int y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetWindowPositionCallback(SetWindowPosition), new object[] { x, y });
            }
            else
            {
                Left = x;
                Top = y;
            }

        }
        delegate void SetWindowPositionCallback(int x, int y);

        public int GetWidth()
        {
            if (!Dispatcher.CheckAccess())
            {
                return (int)Dispatcher.Invoke(new GetWidthCallback(GetWidth));
            }
            else
            {
                return (int)Width;
            }
        }
        delegate int GetWidthCallback();

        public int GetHeight()
        {
            if (!Dispatcher.CheckAccess())
            {
                return (int)Dispatcher.Invoke(new GetHeightCallback(GetHeight));
            }
            else
            {
                return (int)Height;
            }
        }
        delegate int GetHeightCallback();

        private void TabControlLeagueOverlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPage = tabControlLeagueOverlay.SelectedItem as TabItem;
            SetCurrentPageWindowSize();
        }

        private void SetCurrentPageWindowSize()
        {
            if (CurrentPage != null)
            {
                if (!tabPageSizeDictionary.TryGetValue(CurrentPage, out var windowSize))
                {
                    return;
                    // throw new Exception("Window Size for TabPage is not defined correctly");
                }

                Height = windowSize[0];
                Width = windowSize[1];
                tabControlLeagueOverlay.Height = windowSize[0];
                tabControlLeagueOverlay.Width = windowSize[1];
            }
        }

        private void SetTextBlockList(TextBlock block, IEnumerable<string> items)
        {
            block.Inlines.Clear();

            foreach (var item in items)
            {
                var inlineBlock = new TextBlock();
                inlineBlock.Text = item;
                inlineBlock.SetResourceReference(Control.ForegroundProperty, DelveFossilRarityDictionary[item.Replace("*", "")]);
                block.Inlines.Add(inlineBlock);
                block.Inlines.Add("\n");
            }

            block.Inlines.Remove(block.Inlines.LastOrDefault());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void UpdateFossilRarityDictionary()
        {
            DelveFossilRarityDictionary = new Dictionary<string, string>()
            {
                { DelveResources.AberrantFossil, LowValueColorName },
                { DelveResources.AethericFossil, VeryLowValueColorName },
                { DelveResources.BloodstainedFossil, HighValueColorName },
                { DelveResources.BoundFossil, LowValueColorName },
                { DelveResources.CorrodedFossil, MediumValueColorName },
                { DelveResources.DenseFossil, LowValueColorName},
                { DelveResources.EnchantedFossil, MediumValueColorName },
                { DelveResources.EncrustedFossil, VeryLowValueColorName },
                { DelveResources.FacetedFossil, HighValueColorName },
                { DelveResources.FracturedFossil, HighValueColorName },
                { DelveResources.FrigidFossil, VeryLowValueColorName },
                { DelveResources.GildedFossil, MediumValueColorName },
                { DelveResources.GlyphicFossil, HighValueColorName },
                { DelveResources.HollowFossil, HighValueColorName },
                { DelveResources.JaggedFossil, VeryLowValueColorName },
                { DelveResources.LucentFossil, VeryLowValueColorName },
                { DelveResources.MetallicFossil, LowValueColorName },
                { DelveResources.PerfectFossil, MediumValueColorName },
                { DelveResources.PrismaticFossil, LowValueColorName },
                { DelveResources.PristineFossil, VeryLowValueColorName },
                { DelveResources.SanctifiedFossil, HighValueColorName },
                { DelveResources.ScorchedFossil, VeryLowValueColorName },
                { DelveResources.SerratedFossil, MediumValueColorName },
                { DelveResources.ShudderingFossil, MediumValueColorName },
                { DelveResources.TangledFossil, MediumValueColorName },
            };
        }

        private void UpdateHeaderUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            tabItemIncursion.Header = LeagueResources.LeagueNameIncrusion;
            tabItemDelve.Header = LeagueResources.LeagueNameDelve;
            tabItemBetrayal.Header = LeagueResources.LeagueNameBetrayal;
            tabItemBlight.Header = LeagueResources.LeagueNameBlight;
            tabItemMetamorph.Header = LeagueResources.LeagueNameMetamorph;
        }

        private void UpdateIncursionUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelIncrusionLengend.Content = IncursionResources.Legend;

            labelIncursionHeaderContains.Content = IncursionResources.HeaderContains;
            labelIncursionHeaderModifiers.Content = IncursionResources.HeaderModifiers;

            textBlockGuardhouse.Text = IncursionResources.Guardhouse;
            textBlockBarracks.Text = IncursionResources.Barracks;
            textBlockHallOfWar.Text = IncursionResources.HallOfWar;
            textBlockGuardhouseModifiers.Text = IncursionResources.GuardhouseModifiers;

            textBlockWorkshop.Text = IncursionResources.Workshop;
            textBlockWorkshopContains.Text = IncursionResources.WorkshopContains;
            textBlockWorkshopModifiers.Text = IncursionResources.WorkshopModifiers;
            textBlockEngineeringDepartment.Text = IncursionResources.EngineeringDepartment;
            textBlockFactory.Text = IncursionResources.Factory;

            textBlockExplosivesRoom.Text = IncursionResources.ExplosivesRoom;
            textBlockExplosivesRoomContains.Text = IncursionResources.ExplosivesRoomContains;
            textBlockDemolitionLab.Text = IncursionResources.DemolitionLab;
            textBlockShrineOfUnmaking.Text = IncursionResources.ShrineOfUnmaking;

            textBlockSplinterReasearchLab.Text = IncursionResources.SplinterResearchLab;
            textBlockSplinterResearchLabContains.Text = IncursionResources.SplinterResearchLabContains;
            textBlockBreachContainmentChamber.Text = IncursionResources.BreachContainmentChamber;
            textBlockHouseOfTheOthers.Text = IncursionResources.HouseOfOthers;

            textBlockVault.Text = IncursionResources.Vault;
            textBlockVaultContains.Text = IncursionResources.VaultContains;
            textBlockTreasury.Text = IncursionResources.Treasury;
            textBlockWealthOfTheVaal.Text = IncursionResources.WealthOfTheVaal;

            textBlockSparringRoom.Text = IncursionResources.SparringRoom;
            textBlockSparringRoomContains.Text = IncursionResources.SparringRoomContains;
            textBlockSparringRoomModifiers.Text = IncursionResources.SparringRoomModifiers;
            textBlockArenaOfValour.Text = IncursionResources.ArenaOfValour;
            textBlockHallOfChampions.Text = IncursionResources.HallOfChampions;

            textBlockArmourersWorkshop.Text = IncursionResources.ArmourersWorkshop;
            textBlockArmourersWorkshopContains.Text = IncursionResources.ArmourersWorkshopContains;
            textBlockArmourersWorkshopModifiers.Text = IncursionResources.ArmourersWorkshopModifiers;
            textBlockArmoury.Text = IncursionResources.Armoury;
            textBlockChamberOfIron.Text = IncursionResources.ChamberOfIron;

            textBlockJewellersWorkshop.Text = IncursionResources.JewellersWorkshop;
            textBlockJewellersWorkshopContains.Text = IncursionResources.JewellersWorkshopContains;
            textBlockJewelleryForge.Text = IncursionResources.JewelleryForge;
            textBlockGlitteringHalls.Text = IncursionResources.GlitteringHalls;

            textBlockSurveyorsStudy.Text = IncursionResources.SurveyorsStudy;
            textBlockSurveyorsStudyContains.Text = IncursionResources.SurveyorsStudyContains;
            textBlockOfficeOfCartography.Text = IncursionResources.OfficeOfCartography;
            textBlockAtlasOfWorlds.Text = IncursionResources.AtlasOfWorlds;

            textBlockGemcuttersWorkshop.Text = IncursionResources.GemcuttersWorkshop;
            textBlockGemcuttersWorkshopContains.Text = IncursionResources.GemcuttersWorkshopContains;
            textBlockDepartmentOfThaumaturgy.Text = IncursionResources.DepartmentOfThaumaturgy;
            textBlockDoryanisInstitute.Text = IncursionResources.DoryanisInstitute;
            textBlockGemcuttersWorkshopContains.ToolTip = IncursionResources.DoubleGemCorruptionTooltip;

            textBlockTormentCell.Text = IncursionResources.TormentCells;
            textBlockTormentCellsContain.Text = IncursionResources.TormentCellsContains;
            textBlockTortureCages.Text = IncursionResources.TortureCages;
            textBlockSadistsDen.Text = IncursionResources.SadistsDen;

            textBlockStrongboxChamber.Text = IncursionResources.StrongboxChamber;
            textBlockStrongboxChamberContains.Text = IncursionResources.StrongboxChamberContains;
            textBlockHallOfLocks.Text = IncursionResources.HallOfLocks;
            textBlockCourtOfSealedDeath.Text = IncursionResources.CourtOfTheSealedDeath;

            textBlockHallOfMettle.Text = IncursionResources.HallOfMettle;
            textBlockHallOfMettleContains.Text = IncursionResources.HallOfMettleContains;
            textBlockHallOfHeroes.Text = IncursionResources.HallOfHeroes;
            textBlockHallOfLegends.Text = IncursionResources.HallOfLegends;

            textBlockSacrificalChamber.Text = IncursionResources.SacrificialChamber;
            textBlockSacrificalChamberContains.Text = IncursionResources.SacrificalChamberContains;
            textBlockHallOfOfferings.Text = IncursionResources.HallOfOfferings;
            textBlockApexOfAscension.Text = IncursionResources.ApexOfAscension;

            textBlockStorageRoom.Text = IncursionResources.StorageRoom;
            textBlockStorageRoomContains.Text = IncursionResources.StorageRoomContains;
            textBlockWarehouses.Text = IncursionResources.Warehouses;
            textBlockMuseumOfArtifacts.Text = IncursionResources.MuseumOfArtifacts;

            textBlockCorruptionChamber.Text = IncursionResources.CorruptionChamber;
            textBlockCorruptionChamberContains.Text = IncursionResources.CorruptionChamberContains;
            textBlockCorruptionChamberModifiers.Text = IncursionResources.CorruptionChamberModifiers;
            textBlockCatalystOfCorruption.Text = IncursionResources.CatalystOfCorruption;
            textBlockLocusOfCorruption.Text = IncursionResources.LocuOfCorruption;
            textBlockCorruptionChamberContains.ToolTip = IncursionResources.DoubleCorruptionTooltip;

            textBlockShrineOfEmpowerment.Text = IncursionResources.ShrineOfEmpowerment;
            textBlockShrineOfEmpowermentContains.Text = IncursionResources.ShrineOfEmpowermentContains;
            textBlockShrineOfEmpowermentModifiers.Text = IncursionResources.ShrineOfEmpowermentModifiers;
            textBlockSanctumOfUnity.Text = IncursionResources.SanctumOfUnity;
            textBlockTempleNexus.Text = IncursionResources.TempleNexus;

            textBlockTempestGenerator.Text = IncursionResources.TempestGenerator;
            textBlockTempestGeneratorContains.Text = IncursionResources.TempestGeneratorContains;
            textBlockTempestGeneratorModifiers.Text = IncursionResources.TempestGeneratorModifiers;
            textBlockHurricaneEngine.Text = IncursionResources.HurricaneEngine;
            textBlockStormOfCorruption.Text = IncursionResources.StormOfCorruption;
            textBlockTempestGeneratorContains.ToolTip = IncursionResources.TopotanteModTooltip;

            textBlockPosionGarden.Text = IncursionResources.PoisionGarden;
            textBlockPosiionGardenContains.Text = IncursionResources.PosionGardenContains;
            textBlockPosionGardenModifiers.Text = IncursionResources.PoisonGardenModifiers;
            textBlockCultivarChamber.Text = IncursionResources.CultivarChamber;
            textBlockToxicGrive.Text = IncursionResources.ToxicGrove;
            textBlockPosiionGardenContains.ToolTip = IncursionResources.TacatiModTooltip;

            textBlockTrapWorkshop.Text = IncursionResources.TrapWorkshop;
            textBlockTrapWorkshopContains.Text = IncursionResources.TrapWorkshopContains;
            textBlockTrapWorkshopModifiers.Text = IncursionResources.TrapWorkshopModifiers;
            textBlockTempleDefenseWorkshop.Text = IncursionResources.TempleDefenseWorkshop;
            textBlockDefenseResearchLab.Text = IncursionResources.DefenseResearchLab;
            textBlockTrapWorkshopContains.ToolTip = IncursionResources.MatalTooltip;

            textBlockPoolsOfRestoration.Text = IncursionResources.PoolsOfRestoration;
            textBlockPoolsOfRestorationContains.Text = IncursionResources.PoolsOfRestorationContains;
            textBlockPoolsOfRestorationModifiers.Text = IncursionResources.PoolsOfRestorationModifiers;
            textBlockSanctumOfVitality.Text = IncursionResources.SanctumOfVitality;
            textBlockSanctumOfImmortality.Text = IncursionResources.SanctumOfImmortality;
            textBlockPoolsOfRestorationContains.ToolTip = IncursionResources.GuateliztzModTooltip;

            textBlockFlameWorkshop.Text = IncursionResources.FlameWorkshop;
            textBlockFlameWorkshopContains.Text = IncursionResources.FlameWorkshopContains;
            textBlockFlameWorkshopModifiers.Text = IncursionResources.FlameWorkshopModifiers;
            textBlockOmnitectForge.Text = IncursionResources.OmnitectForge;
            textBlockCrucibleOfFlame.Text = IncursionResources.CrucibleOfFlame;
            textBlockFlameWorkshopContains.ToolTip = IncursionResources.PuhuarteModTooltuip;

            textBlockLightningWorkshop.Text = IncursionResources.LightningWorkshop;
            textBlockLightningWorkshopContains.Text = IncursionResources.LightningWorkshopContains;
            textBlockLightningWorkshopModifiers.Text = IncursionResources.LightningWorkshopModifiers;
            textBlockOmnitectReactorPlant.Text = IncursionResources.OmnitectReactorPlant;
            textBlockConduitOfLightning.Text = IncursionResources.ConduitOfLightning;
            textBlockLightningWorkshopContains.ToolTip = IncursionResources.XopecModTooltip;

            textBlockHatchery.Text = IncursionResources.Hatchery;
            textBlockHatcherModifiers.Text = IncursionResources.HatcheryModifiers;
            textBlockHatcheryContains.Text = IncursionResources.HatcheryContains;
            textBlockAutomationLab.Text = IncursionResources.AutomatonLab;
            textBlockHybridisationChamber.Text = IncursionResources.HybridisationChamber;
            textBlockHatcheryContains.ToolTip = IncursionResources.CitaqualotlModTooltip;

            textBlockRoyalMeetingRoom.Text = IncursionResources.RoyalMeetingRoom;
            textBlockRoyalMeetingRoomContains.Text = IncursionResources.RoyalMeetingRoomContains;
            textBlockRoyalMeetingRoomModifiers.Text = IncursionResources.RoyalMeetingRoomModifiers;
            textBlockHallOfLords.Text = IncursionResources.HallOfLords;
            textBlockThroneOfAtziri.Text = IncursionResources.ThroneOfAtziri;
        }

        private void UpdateBetrayalUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelBetrayalLegendNotValuable.Content = LeagueResources.LegendNotValuable;
            labelBetrayalLegendLessValuable.Content = LeagueResources.LegendLessValuable;
            labelBetrayalLegendValuable.Content = LeagueResources.LegendValuable;
            labelBetrayalLegendMoreValuable.Content = LeagueResources.LegendMoreValuable;

            labelBetrayalTypeTransportation.Content = BetrayalResources.TypeTransportation;
            labelBetrayalTypeResearch.Content = BetrayalResources.TypeResearch;
            labelBetrayalTypeFortification.Content = BetrayalResources.TypeFortification;
            labelBetrayalTypeIntervention.Content = BetrayalResources.TypeIntervention;

            labelAislingName.Content = BetrayalResources.AislingName;
            textBlockAislingTransportation.Text = BetrayalResources.AislingTransportaion;
            textBlockAislingFortification.Text = BetrayalResources.AislingFortification;
            textBlockAislingResearch.Text = BetrayalResources.AislingResearch;
            textBlockAislingResearch.ToolTip = BetrayalResources.AislingResearchTooltip;
            textBlockAislingIntervention.Text = BetrayalResources.AislingIntervention;

            labelCameriaName.Content = BetrayalResources.CameriaName;
            textBlockCameriaFortification.Text = BetrayalResources.CameriaFortification;
            textBlockCameriaIntervention.Text = BetrayalResources.CameriaIntervention;
            textBlockCameriaResearch.Text = BetrayalResources.CameriaResearch;
            textBlockCameriaTransportation.Text = BetrayalResources.CameriaTransportation;
            textBlockCameriaTransportation.ToolTip = BetrayalResources.CameriaTransportationTooltip;

            labelElreonName.Content = BetrayalResources.ElreonName;
            textBlockElreonFortification.Text = BetrayalResources.ElreonFortification;
            textBlockElreonIntervention.Text = BetrayalResources.ElreonIntervention;
            textBlockElreonResearch.Text = BetrayalResources.ElreonResearch;
            textBlockElreonTransportation.Text = BetrayalResources.ElreonTransportation;

            labelGraviciusName.Content = BetrayalResources.GraviciusName;
            textBlockGraviciusFortification.Text = BetrayalResources.GraviciusFortification;
            textBlockGraviciusIntervention.Text = BetrayalResources.GraviciusIntervention;
            textBlockGraviciusResearch.Text = BetrayalResources.GraviciusResearch;
            textBlockGraviciusTransportation.Text = BetrayalResources.GraviciusTransportation;

            labelGuffName.Content = BetrayalResources.GuffName;
            textBlockGuffFortification.Text = BetrayalResources.GuffFortification;
            textBlockGuffFortification.ToolTip = BetrayalResources.GuffFortificationTooltip;
            textBlockGuffIntervention.Text = BetrayalResources.GuffIntervention;
            textBlockGuffIntervention.ToolTip = BetrayalResources.GuffInterventionTooltip;
            textBlockGuffResearch.Text = BetrayalResources.GuffResearch;
            textBlockGuffResearch.ToolTip = BetrayalResources.GuffResearchTooltip;
            textBlockGuffTransportation.Text = BetrayalResources.GuffTransportation;
            textBlockGuffTransportation.ToolTip = BetrayalResources.GuffTransportationTooltip;

            labelHakuName.Content = BetrayalResources.HakuName;
            textBlockHakuFortification.Text = BetrayalResources.HakuFortification;
            textBlockHakuIntervention.Text = BetrayalResources.HakuIntervention;
            textBlockHakuResearch.Text = BetrayalResources.HakuResearch;
            textBlockHakuTransportation.Text = BetrayalResources.HakuTransportation;

            labelHillockName.Content = BetrayalResources.HillockName;
            textBlockHillockFortification.Text = BetrayalResources.HillockFortification;
            textBlockHillockFortification.ToolTip = BetrayalResources.HillockFortificationTooltip;
            textBlockHillockIntervention.Text = BetrayalResources.HillockIntervention;
            textBlockHillockIntervention.ToolTip = BetrayalResources.HillockInterventionTooltip;
            textBlockHillockResearch.Text = BetrayalResources.HillockResearch;
            textBlockHillockResearch.ToolTip = BetrayalResources.HillockResearchTooltip;
            textBlockHillockTransportation.Text = BetrayalResources.HillockTransportation;
            textBlockHillockTransportation.ToolTip = BetrayalResources.HillockTransportationTooltip;

            labelItThatFledName.Content = BetrayalResources.ItThatFledName;
            textBlockItThatFledFortification.Text = BetrayalResources.ItThatFledFortification;
            textBlockItThatFledIntervention.Text = BetrayalResources.ItThatFledIntervention;
            textBlockItThatFledResearch.Text = BetrayalResources.ItThatFledResearch;
            textBlockItThatFledResearch.ToolTip = BetrayalResources.ItThatFledResearchTooltip;
            textBlockItThatFledTransportation.Text = BetrayalResources.ItThatFledTransportation;

            labelJanusName.Content = BetrayalResources.JanusName;
            textBlockJanusFortification.Text = BetrayalResources.JanusFortification;
            textBlockJanusIntervention.Text = BetrayalResources.JanusIntervention;
            textBlockJanusResearch.Text = BetrayalResources.JanusResearch;
            textBlockJanusTransportation.Text = BetrayalResources.JanusTransportation;

            labelJorginName.Content = BetrayalResources.JorginName;
            textBlockJorginFortification.Text = BetrayalResources.JorginFortification;
            textBlockJorginIntervention.Text = BetrayalResources.JorginIntervention;
            textBlockJorginResearch.Text = BetrayalResources.JorginResearch;
            textBlockJorginResearch.ToolTip = BetrayalResources.JorginResearchTooltip;
            textBlockJorginTransportation.Text = BetrayalResources.JorginTransportation;

            labelKorellName.Content = BetrayalResources.KorellName;
            textBlockKorellFortifcation.Text = BetrayalResources.KorellFortification;
            textBlockKorellIntervention.Text = BetrayalResources.KorellIntervention;
            textBlockKorellResearch.Text = BetrayalResources.KorellResearch;
            textBlockKorellTransportation.Text = BetrayalResources.KorellTransportation;

            labelLeoName.Content = BetrayalResources.LeoName;
            textBlockLeoFortification.Text = BetrayalResources.LeoFortification;
            textBlockLeoIntervention.Text = BetrayalResources.LeoIntervention;
            textBlockLeoResearch.Text = BetrayalResources.LeoResearch;
            textBlockLeoResearch.ToolTip = BetrayalResources.LeoResearchTooltip;
            textBlockLeoTransportation.Text = BetrayalResources.LeoTransportation;

            labelRikerName.Content = BetrayalResources.RikerName;
            textBlockRikerFortification.Text = BetrayalResources.RikerFortification;
            textBlockRikerIntervention.Text = BetrayalResources.RikerIntervention;
            textBlockRikerResearch.Text = BetrayalResources.RikerResearch;
            textBlockRikerTransportation.Text = BetrayalResources.RikerTransportation;

            labelRinName.Content = BetrayalResources.RinName;
            textBlockRinFortification.Text = BetrayalResources.RinFortification;
            textBlockRinIntervention.Text = BetrayalResources.RinIntervention;
            textBlockRinResearch.Text = BetrayalResources.RinResearch;
            textBlockRinTransportation.Text = BetrayalResources.RinTransportation;

            labelToraName.Content = BetrayalResources.ToraName;
            textBlockToraFortification.Text = BetrayalResources.ToraFortification;
            textBlockToraFortification.ToolTip = BetrayalResources.ToraFortificationTooltip;
            textBlockToraIntervention.Text = BetrayalResources.ToraIntervention;
            textBlockToraResearch.Text = BetrayalResources.ToraResearch;
            textBlockToraResearch.ToolTip = BetrayalResources.ToraResearchTooltip;
            textBlockToraTransportation.Text = BetrayalResources.ToraTransportation;
            textBlockToraTransportation.ToolTip = BetrayalResources.ToraTransportationTooltip;

            labelVaganName.Content = BetrayalResources.VaganName;
            textBlockVaganFortification.Text = BetrayalResources.VaganFortification;
            textBlockVaganIntervention.Text = BetrayalResources.VaganIntervention;
            textBlockVaganResearch.Text = BetrayalResources.VaganResearch;
            textBlockVaganTransportation.Text = BetrayalResources.VaganTransportation;

            labelVoriciName.Content = BetrayalResources.VoriciName;
            textBlockVoriciFortification.Text = BetrayalResources.VoriciFortification;
            textBlockVoriciIntervention.Text = BetrayalResources.VoriciIntervention;
            textBlockVoriciResearch.Text = BetrayalResources.VoriciResearch;
            textBlockVoriciResearch.ToolTip = BetrayalResources.VoriciResearchTooltip;
            textBlockVoriciTransportation.Text = BetrayalResources.VoriciTransportation;
        }

        private void UpdateBlightUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelBlightLegend.Content = BlightResources.Legend;

            labelClearOil.Content = BlightResources.ClearOil;
            textBlockClearOilEffect.Text = BlightResources.ClearOilEffect;

            labelSepiaOil.Content = BlightResources.SepiaOil;
            textBlockSepiaOilEffect.Text = BlightResources.SepiaOilEffect;

            labelAmberOil.Content = BlightResources.AmberOil;
            textBlockAmberOilEffect.Text = BlightResources.AmberOilEffect;

            labelVerdantOil.Content = BlightResources.VerdantOil;
            textBlockVerdantOilEffect.Text = BlightResources.VerdantOilEffect;

            labelTealOil.Content = BlightResources.TealOil;
            textBlockTealOilEffect.Text = BlightResources.TealOilEffect;

            labelAzureOil.Content = BlightResources.AzureOil;
            textBlockAzureOilEffect.Text = BlightResources.AzureOilEffect;

            labelVioletOil.Content = BlightResources.VioletOil;
            textBlockVioletOilEffect.Text = BlightResources.VioletOilEffect;

            labelCrimsonOil.Content = BlightResources.CrimsonOil;
            textBlockCrimsonOilEffect.Text = BlightResources.CrimsonOilEffect;

            labelBlackOil.Content = BlightResources.BlackOil;
            textBlockBlackOilEffect.Text = BlightResources.BlackOilEffect;

            labelOpalescentOil.Content = BlightResources.OpalescentOil;
            textBlockOpalescentOilEffect.Text = BlightResources.OpalescentOilEffect;

            labelSilverOil.Content = BlightResources.SilverOil;
            textBlockSilverOilEffect.Text = BlightResources.SilverOilEffect;

            labelGoldenOil.Content = BlightResources.GoldenOil;
            textBlockGoldenOilEffect.Text = BlightResources.GoldenOilEffect;
        }

        private void UpdateMetamorphUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelAbrasiveCatalyst.Content = MetamorphResources.AbrasiveCatalyst;
            textBlockAbrasiveCatalystEffect.Text = MetamorphResources.AbrasiveCatalystEffect;

            labelFertileCatalyst.Content = MetamorphResources.FertileCatalyst;
            textBlockFertileCatalystEffect.Text = MetamorphResources.FertileCatalyst;

            labelImbuedCatalyst.Content = MetamorphResources.ImbuedCatalyst;
            textBlockImbuedCatalystEffect.Text = MetamorphResources.ImbuedCatalystEffect;

            labelIntrinsicCatalyst.Content = MetamorphResources.IntrinsicCatalyst;
            textBlockIntrinsicCatalystEffect.Text = MetamorphResources.IntrinsicCatalystEffect;

            labelPrismaticCatalyst.Content = MetamorphResources.PrismaticCatalyst;
            textBlockPrismaticCatalystEffect.Text = MetamorphResources.PrismaticCatalystEffect;

            labelTemperingCatalyst.Content = MetamorphResources.TemperingCatalyst;
            textBlockTemperingCatalystEffect.Text = MetamorphResources.TemperingCatalystEffect;

            labelTurbulentCatalyst.Content = MetamorphResources.TurbulentCatalyst;
            textBlockTurbulentCatalystEffect.Text = MetamorphResources.TurbulentCatalystEffect;

            labelMetamorphInformation.Content = MetamorphResources.InformationHeader;
            textBlockMetamorphInformationText.Text = MetamorphResources.InformationText;
        }

        private void UpdateDelveUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            string fracturedWallInfoPointer = "*";

            var mineFossils = new[]
            {
                DelveResources.MetallicFossil,
                DelveResources.SerratedFossil,
                DelveResources.PristineFossil,
                DelveResources.AethericFossil,
            };

            var magmaFissureFossils = new[]
            {
                DelveResources.ScorchedFossil,
                DelveResources.PristineFossil,
                DelveResources.PrismaticFossil,
                DelveResources.EnchantedFossil,
                DelveResources.EncrustedFossil + fracturedWallInfoPointer,
            };

            var sulfurVentsFossils = new[]
            {
                DelveResources.MetallicFossil,
                DelveResources.AethericFossil,
                DelveResources.PerfectFossil,
                DelveResources.EncrustedFossil + fracturedWallInfoPointer,
            };

            var frozenHollowFossils = new[]
            {
                DelveResources.FrigidFossil,
                DelveResources.SerratedFossil,
                DelveResources.PrismaticFossil,
                DelveResources.ShudderingFossil,
                DelveResources.SanctifiedFossil + fracturedWallInfoPointer,
            };

            var fungalCavernsFossils = new[]
            {
                DelveResources.DenseFossil,
                DelveResources.AberrantFossil,
                DelveResources.PerfectFossil,
                DelveResources.CorrodedFossil,
                DelveResources.GildedFossil + fracturedWallInfoPointer,
            };

            var petrifiedForestFossils = new[]
            {
                DelveResources.BoundFossil,
                DelveResources.DenseFossil,
                DelveResources.JaggedFossil,
                DelveResources.CorrodedFossil,
                DelveResources.SanctifiedFossil + fracturedWallInfoPointer,
            };

            var abyssalDepthsFossils = new[]
            {
                DelveResources.BoundFossil,
                DelveResources.AberrantFossil,
                DelveResources.LucentFossil + fracturedWallInfoPointer,
                DelveResources.GildedFossil + fracturedWallInfoPointer,
            };

            var fossilRoomFossils = new[]
            {
                DelveResources.GlyphicFossil,
                DelveResources.FracturedFossil,
                DelveResources.FacetedFossil,
                DelveResources.BloodstainedFossil,
                DelveResources.TangledFossil,
                DelveResources.HollowFossil,
            };

            labelDelveMines.Content = DelveResources.Mines;
            SetTextBlockList(textBlockDelveMinesFossils, mineFossils);

            labelDelveMagmaFissure.Content = DelveResources.MagmaFissure;
            SetTextBlockList(textBlockMagmaFissureFossils, magmaFissureFossils);

            labelDelveSulfurVents.Content = DelveResources.SulfurVents;
            SetTextBlockList(textBlockSulfurVentsFossils, sulfurVentsFossils);

            labelDelveFrozenHollow.Content = DelveResources.FrozenHollow;
            SetTextBlockList(textBlockFrozenHolloeFossils, frozenHollowFossils);

            labelDelveFungalCaverns.Content = DelveResources.FungalCaverns;
            SetTextBlockList(textBlockDelveFungalCavernsFossils, fungalCavernsFossils);

            labelDelvePetrifiedForest.Content = DelveResources.PetrifiedForest;
            SetTextBlockList(labelDelvePetrifiedForestFossils, petrifiedForestFossils);

            labelDelveAbyssalDepths.Content = DelveResources.AbyssalDepths;
            SetTextBlockList(textBlockAbyssalDepthsFossils, abyssalDepthsFossils);

            labelDelveFossilRoom.Content = DelveResources.FossilRoom;
            SetTextBlockList(textBlockDelveFossilRoomFossils, fossilRoomFossils);

            labelDelveInformation.Content = DelveResources.Information;
            labelDelveLegend.Content = DelveResources.Legend;
            labelDelveLegendNotValuable.Content = LeagueResources.LegendNotValuable;
            labelDelveLegendLowValuable.Content = LeagueResources.LegendLessValuable;
            labelDelveLegendValuable.Content = LeagueResources.LegendValuable;
            labelDelveLegendMoreValuable.Content = LeagueResources.LegendMoreValuable;
        }
    }
}
