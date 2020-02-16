using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Sidekick.Localization;
using Sidekick.Localization.Leagues;
using Sidekick.Localization.Leagues.Incursion;

namespace Sidekick.Windows.LeagueOverlay
{
    /// <summary>
    /// Interaction logic for LeagueOverlayView.xaml
    /// </summary>
    public partial class LeagueOverlayView : Window
    {
        private readonly IUILanguageProvider languageProvider;
        private Dictionary<TabItem, int[]> tabPageSizeDictionary;
        private TabItem CurrentPage;
        private Dictionary<string, string> DelveFossilRarityDictionary;

        public const string VeryLowValueColorName = "VeryLowValueColor";
        public const string LowValueColorName = "LowValueColor";
        public const string MediumValueColorName = "MediumValueColor";
        public const string HighValueColorName = "HighValueColor";

        public LeagueOverlayView(IUILanguageProvider languageProvider)
        {
            this.languageProvider = languageProvider;
            InitializeComponent();

            UpdateHeaderUIText();
            UpdateIncursionUIText();

            languageProvider.UILanguageChanged += UpdateHeaderUIText;
            languageProvider.UILanguageChanged += UpdateIncursionUIText;

            tabPageSizeDictionary = new Dictionary<TabItem, int[]>()
            {
                { tabItemIncursion, new[] { 980, 1050 } },
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

        private void UpdateHeaderUIText()
        {
            var cultureInfo = new CultureInfo(languageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            tabItemIncursion.Header = LeagueResources.LeagueNameIncrusion;
        }

        private void UpdateIncursionUIText()
        {
            var cultureInfo = new CultureInfo(languageProvider.Current.Name);
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
    }
}
