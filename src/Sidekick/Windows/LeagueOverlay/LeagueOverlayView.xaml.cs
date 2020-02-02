using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Sidekick.Localization.Leagues.Betrayal;

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

            DelveFossilRarityDictionary = new Dictionary<string, string>()
            {
                { Legacy.UILanguageProvider.Language.DelveAberrantFossil, LowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveAethericFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveBloodstainedFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveBoundFossil, LowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveCorrodedFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelveDenseFossil, LowValueColorName},
                { Legacy.UILanguageProvider.Language.DelveEnchantedFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelveEncrustedFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveFacetedFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveFracturedFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveFrigidFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveGildedFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelveGlyphicFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveHollowFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveJaggedFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveLucentFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveMetallicFossil, LowValueColorName },
                { Legacy.UILanguageProvider.Language.DelvePerfectFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelvePrismaticFossil, LowValueColorName },
                { Legacy.UILanguageProvider.Language.DelvePristineFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveSanctifiedFossil, HighValueColorName },
                { Legacy.UILanguageProvider.Language.DelveScorchedFossil, VeryLowValueColorName },
                { Legacy.UILanguageProvider.Language.DelveSerratedFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelveShudderingFossil, MediumValueColorName },
                { Legacy.UILanguageProvider.Language.DelveTangledFossil, MediumValueColorName },
            };


            UpdateBetrayalUIText();
            UpdateIncursionUIText();
            UpdateBlightUIText();
            UpdateMetamorphUIText();
            UpdateDelveUIText();
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
                    throw new Exception("Window Size for TabPage is not defined correctly");
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

        private void UpdateIncursionUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelIncursionHeaderContains.Content = Legacy.UILanguageProvider.Language.IncursionHeaderContains;
            labelIncursionHeaderModifiers.Content = Legacy.UILanguageProvider.Language.IncursionHeaderModifiers;

            textBlockGuardhouse.Text = Legacy.UILanguageProvider.Language.IncursionGuardhouse;
            textBlockBarracks.Text = Legacy.UILanguageProvider.Language.IncursionBarracks;
            textBlockHallOfWar.Text = Legacy.UILanguageProvider.Language.IncursionHallOfWar;
            textBlockGuardhouseModifiers.Text = Legacy.UILanguageProvider.Language.IncursionGuardhouseModifiers;

            textBlockWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionWorkshop;
            textBlockWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionWorkshopContains;
            textBlockWorkshopModifiers.Text = Legacy.UILanguageProvider.Language.IncursionWorkshopModifiers;
            textBlockEngineeringDepartment.Text = Legacy.UILanguageProvider.Language.IncursionEngineeringDepartment;
            textBlockFactory.Text = Legacy.UILanguageProvider.Language.IncursionFactory;

            textBlockExplosivesRoom.Text = Legacy.UILanguageProvider.Language.IncursionExplosivesRoom;
            textBlockExplosivesRoomContains.Text = Legacy.UILanguageProvider.Language.IncursionExplosivesRoomContains;
            textBlockDemolitionLab.Text = Legacy.UILanguageProvider.Language.IncursionDemolitionLab;
            textBlockShrineOfUnmaking.Text = Legacy.UILanguageProvider.Language.IncursionShrineOfUnmaking;

            textBlockSplinterReasearchLab.Text = Legacy.UILanguageProvider.Language.IncursionSplinterResearchLab;
            textBlockSplinterResearchLabContains.Text = Legacy.UILanguageProvider.Language.IncursionSplinterResearchLabContains;
            textBlockBreachContainmentChamber.Text = Legacy.UILanguageProvider.Language.IncursionBreachContainmentChamber;
            textBlockHouseOfTheOthers.Text = Legacy.UILanguageProvider.Language.IncursionHouseOfOthers;

            textBlockVault.Text = Legacy.UILanguageProvider.Language.IncursionVault;
            textBlockVaultContains.Text = Legacy.UILanguageProvider.Language.IncursionVaultContains;
            textBlockTreasury.Text = Legacy.UILanguageProvider.Language.IncursionTreasury;
            textBlockWealthOfTheVaal.Text = Legacy.UILanguageProvider.Language.IncursionWealthOfTheVaal;

            textBlockSparringRoom.Text = Legacy.UILanguageProvider.Language.IncursionSparringRoom;
            textBlockSparringRoomContains.Text = Legacy.UILanguageProvider.Language.IncursionSparringRoomContains;
            textBlockSparringRoomModifiers.Text = Legacy.UILanguageProvider.Language.IncursionSparringRoomModifiers;
            textBlockArenaOfValour.Text = Legacy.UILanguageProvider.Language.IncursionArenaOfValour;
            textBlockHallOfChampions.Text = Legacy.UILanguageProvider.Language.IncursionHallOfChampions;

            textBlockArmourersWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionArmourersWorkshop;
            textBlockArmourersWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionArmourersWorkshopContains;
            textBlockArmourersWorkshopModifiers.Text = Legacy.UILanguageProvider.Language.IncursionArmourersWorkshopModifiers;
            textBlockArmoury.Text = Legacy.UILanguageProvider.Language.IncursionArmoury;
            textBlockChamberOfIron.Text = Legacy.UILanguageProvider.Language.IncursionChamberOfIron;

            textBlockJewellersWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionJewellersWorkshop;
            textBlockJewellersWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionJewellersWorkshopContains;
            textBlockJewelleryForge.Text = Legacy.UILanguageProvider.Language.IncursionJewelleryForge;
            textBlockGlitteringHalls.Text = Legacy.UILanguageProvider.Language.IncursionGlitteringHalls;

            textBlockSurveyorsStudy.Text = Legacy.UILanguageProvider.Language.IncursionSurveyorsStudy;
            textBlockSurveyorsStudyContains.Text = Legacy.UILanguageProvider.Language.IncursionSurveyorsStudyContains;
            textBlockOfficeOfCartography.Text = Legacy.UILanguageProvider.Language.IncursionOfficeOfCartography;
            textBlockAtlasOfWorlds.Text = Legacy.UILanguageProvider.Language.IncursionAtlasOfWorlds;

            textBlockGemcuttersWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionGemcuttersWorkshop;
            textBlockGemcuttersWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionGemcuttersWorkshopContains;
            textBlockDepartmentOfThaumaturgy.Text = Legacy.UILanguageProvider.Language.IncursionDepartmentOfThaumaturgy;
            textBlockDoryanisInstitute.Text = Legacy.UILanguageProvider.Language.IncursionDoryanisInstitute;
            textBlockGemcuttersWorkshopContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionDoubleGemCorruptionTooltip;

            textBlockTormentCell.Text = Legacy.UILanguageProvider.Language.IncursionTormentCells;
            textBlockTormentCellsContain.Text = Legacy.UILanguageProvider.Language.IncursionTormentCellsContains;
            textBlockTortureCages.Text = Legacy.UILanguageProvider.Language.IncursionTortureCages;
            textBlockSadistsDen.Text = Legacy.UILanguageProvider.Language.IncursionSadistsDen;

            textBlockStrongboxChamber.Text = Legacy.UILanguageProvider.Language.IncursionStrongboxChamber;
            textBlockStrongboxChamberContains.Text = Legacy.UILanguageProvider.Language.IncursionStrongboxChamberContains;
            textBlockHallOfLocks.Text = Legacy.UILanguageProvider.Language.IncursionHallOfLocks;
            textBlockCourtOfSealedDeath.Text = Legacy.UILanguageProvider.Language.IncursionCourtOfTheSealedDeath;

            textBlockHallOfMettle.Text = Legacy.UILanguageProvider.Language.IncursionHallOfMettle;
            textBlockHallOfMettleContains.Text = Legacy.UILanguageProvider.Language.IncursionHallOfMettleContains;
            textBlockHallOfHeroes.Text = Legacy.UILanguageProvider.Language.IncursionHallOfHeroes;
            textBlockHallOfLegends.Text = Legacy.UILanguageProvider.Language.IncursionHallOfLegends;

            textBlockSacrificalChamber.Text = Legacy.UILanguageProvider.Language.IncursionSacrificalChamber;
            textBlockSacrificalChamberContains.Text = Legacy.UILanguageProvider.Language.IncursionSacrificalChamberContains;
            textBlockHallOfOfferings.Text = Legacy.UILanguageProvider.Language.IncursionHallOfOfferings;
            textBlockApexOfAscension.Text = Legacy.UILanguageProvider.Language.IncursionApexOfAscension;

            textBlockStorageRoom.Text = Legacy.UILanguageProvider.Language.IncursionStorageRoom;
            textBlockStorageRoomContains.Text = Legacy.UILanguageProvider.Language.IncursionStorageRoomContains;
            textBlockWarehouses.Text = Legacy.UILanguageProvider.Language.IncursionWarehouses;
            textBlockMuseumOfArtifacts.Text = Legacy.UILanguageProvider.Language.IncursionMuseumOfArtifacts;

            textBlockCorruptionChamber.Text = Legacy.UILanguageProvider.Language.IncursionCorruptionChamber;
            textBlockCorruptionChamberContains.Text = Legacy.UILanguageProvider.Language.IncursionCorruptionChamberContains;
            textBlockCorruptionChamberModifiers.Text = Legacy.UILanguageProvider.Language.IncursionCorruptionChamberModifiers;
            textBlockCatalystOfCorruption.Text = Legacy.UILanguageProvider.Language.IncursionCatalystOfCorruption;
            textBlockLocusOfCorruption.Text = Legacy.UILanguageProvider.Language.IncursionLocuOfCorruption;
            textBlockCorruptionChamberContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionDoubleCorruptionTooltip;

            textBlockShrineOfEmpowerment.Text = Legacy.UILanguageProvider.Language.IncursionShrineOfEmpowerment;
            textBlockShrineOfEmpowermentContains.Text = Legacy.UILanguageProvider.Language.IncursionShrineOfEmpowermentContains;
            textBlockShrineOfEmpowermentModifiers.Text = Legacy.UILanguageProvider.Language.IncursionShrineOfEmpowermentModifiers;
            textBlockSanctumOfUnity.Text = Legacy.UILanguageProvider.Language.IncursionSanctumOfUnity;
            textBlockTempleNexus.Text = Legacy.UILanguageProvider.Language.IncursionTempleNexus;

            textBlockTempestGenerator.Text = Legacy.UILanguageProvider.Language.IncursionTempestGenerator;
            textBlockTempestGeneratorContains.Text = Legacy.UILanguageProvider.Language.IncursionTempestGeneratorContains;
            textBlockTempestGeneratorModifiers.Text = Legacy.UILanguageProvider.Language.IncursionTempestGeneratorModifiers;
            textBlockHurricaneEngine.Text = Legacy.UILanguageProvider.Language.IncursionHurricaneEngine;
            textBlockStormOfCorruption.Text = Legacy.UILanguageProvider.Language.IncursionStormOfCorruption;
            textBlockTempestGeneratorContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionTopotanteModTooltip;

            textBlockPosionGarden.Text = Legacy.UILanguageProvider.Language.IncursionPoisionGarden;
            textBlockPosiionGardenContains.Text = Legacy.UILanguageProvider.Language.IncursionPosionGardenContains;
            textBlockPosionGardenModifiers.Text = Legacy.UILanguageProvider.Language.IncursionPoisonGardenModifiers;
            textBlockCultivarChamber.Text = Legacy.UILanguageProvider.Language.IncursionCultivarChamber;
            textBlockToxicGrive.Text = Legacy.UILanguageProvider.Language.IncursionToxicGrove;
            textBlockPosiionGardenContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionTacatiModTooltip;

            textBlockTrapWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionTrapWorkshop;
            textBlockTrapWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionTrapWorkshopContains;
            textBlockTrapWorkshopModifiers.Text = Legacy.UILanguageProvider.Language.IncursionTrapWorkshopModifiers;
            textBlockTempleDefenseWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionTempleDefenseWorkshop;
            textBlockDefenseResearchLab.Text = Legacy.UILanguageProvider.Language.IncursionDefenseResearchLab;
            textBlockTrapWorkshopContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionMatalTooltip;

            textBlockPoolsOfRestoration.Text = Legacy.UILanguageProvider.Language.IncursionPoolsOfRestoration;
            textBlockPoolsOfRestorationContains.Text = Legacy.UILanguageProvider.Language.IncursionPoolsOfRestorationContains;
            textBlockPoolsOfRestorationModifiers.Text = Legacy.UILanguageProvider.Language.IncursionPoolsOfRestorationModifiers;
            textBlockSanctumOfVitality.Text = Legacy.UILanguageProvider.Language.IncursionSanctumOfVitality;
            textBlockSanctumOfImmortality.Text = Legacy.UILanguageProvider.Language.IncursionSanctumOfImmortality;
            textBlockPoolsOfRestorationContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionGuateliztzModTooltip;

            textBlockFlameWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionFlameWorkshop;
            textBlockFlameWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionFlameWorkshopContains;
            textBlockFlameWorkshopModifiers.Text = Legacy.UILanguageProvider.Language.IncursionFlameWorkshopModifiers;
            textBlockOmnitectForge.Text = Legacy.UILanguageProvider.Language.IncursionOmnitectForge;
            textBlockCrucibleOfFlame.Text = Legacy.UILanguageProvider.Language.IncursionCrucibleOfFlame;
            textBlockFlameWorkshopContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionPuhuarteModTooltuip;

            textBlockLightningWorkshop.Text = Legacy.UILanguageProvider.Language.IncursionLightningWorkshop;
            textBlockLightningWorkshopContains.Text = Legacy.UILanguageProvider.Language.IncursionLightningWorkshopContains;
            textBlockLightningWorkshopModifiers.Text = Legacy.UILanguageProvider.Language.IncursionLightningWorkshopModifiers;
            textBlockOmnitectReactorPlant.Text = Legacy.UILanguageProvider.Language.IncursionOmnitectReactorPlant;
            textBlockConduitOfLightning.Text = Legacy.UILanguageProvider.Language.IncursionConduitOfLightning;
            textBlockLightningWorkshopContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionXopecModTooltip;

            textBlockHatchery.Text = Legacy.UILanguageProvider.Language.IncursionHatchery;
            textBlockHatcherModifiers.Text = Legacy.UILanguageProvider.Language.IncursionHatcheryModifiers;
            textBlockHatcheryContains.Text = Legacy.UILanguageProvider.Language.IncursionHatcheryContains;
            textBlockAutomationLab.Text = Legacy.UILanguageProvider.Language.IncursionAutomationLab;
            textBlockHybridisationChamber.Text = Legacy.UILanguageProvider.Language.IncursionHybridisationChamber;
            textBlockHatcheryContains.ToolTip = Legacy.UILanguageProvider.Language.IncursionCitaqualotlModTooltip;

            textBlockRoyalMeetingRoom.Text = Legacy.UILanguageProvider.Language.IncursionRoyalMeetingRoom;
            textBlockRoyalMeetingRoomContains.Text = Legacy.UILanguageProvider.Language.IncursionRoyalMeetingRoomContains;
            textBlockRoyalMeetingRoomModifiers.Text = Legacy.UILanguageProvider.Language.IncursionRoyalMeetingRoomModifiers;
            textBlockHallOfLords.Text = Legacy.UILanguageProvider.Language.IncursionHallOfLords;
            textBlockThroneOfAtziri.Text = Legacy.UILanguageProvider.Language.IncursionThroneOfAtziri;
        }

        private void UpdateBetrayalUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            textBlockAislingTransportation.Text = BetrayalResources.AislingTransportaion;
            textBlockAislingFortification.Text = BetrayalResources.AislingFortification;
            textBlockAislingResearch.Text = BetrayalResources.AislingResearch;
            textBlockAislingResearch.ToolTip = BetrayalResources.AislingResearchTooltip;
            textBlockAislingIntervention.Text = BetrayalResources.AislingIntervention;

            textBlockCameriaFortification.Text = BetrayalResources.CameriaFortification;
            textBlockCameriaIntervention.Text = BetrayalResources.CameriaIntervention;
            textBlockCameriaResearch.Text = BetrayalResources.CameriaResearch;
            textBlockCameriaTransportation.Text = BetrayalResources.CameriaTransportation;
            textBlockCameriaTransportation.ToolTip = BetrayalResources.CameriaTransportationTooltip;

            textBlockElreonFortification.Text = BetrayalResources.ElreonFortification;
            textBlockElreonIntervention.Text = BetrayalResources.ElreonIntervention;
            textBlockElreonResearch.Text = BetrayalResources.ElreonResearch;
            textBlockElreonTransportation.Text = BetrayalResources.ElreonTransportation;

            textBlockGraviciusFortification.Text = BetrayalResources.GraviciusFortification;
            textBlockGraviciusIntervention.Text = BetrayalResources.GraviciusIntervention;
            textBlockGraviciusResearch.Text = BetrayalResources.GraviciusResearch;
            textBlockGraviciusTransportation.Text = BetrayalResources.GraviciusTransportation;

            textBlockGuffFortification.Text = BetrayalResources.GuffFortification;
            textBlockGuffFortification.ToolTip = BetrayalResources.GuffFortificationTooltip;
            textBlockGuffIntervention.Text = BetrayalResources.GuffIntervention;
            textBlockGuffIntervention.ToolTip = BetrayalResources.GuffInterventionTooltip;
            textBlockGuffResearch.Text = BetrayalResources.GuffResearch;
            textBlockGuffResearch.ToolTip = BetrayalResources.GuffResearchTooltip;
            textBlockGuffTransportation.Text = BetrayalResources.GuffTransportation;
            textBlockGuffTransportation.ToolTip = BetrayalResources.GuffTransportationTooltip;

            textBlockHakuFortification.Text = BetrayalResources.HakuFortification;
            textBlockHakuIntervention.Text = BetrayalResources.HakuIntervention;
            textBlockHakuResearch.Text = BetrayalResources.HakuResearch;
            textBlockHakuTransportation.Text = BetrayalResources.HakuTransportation;

            textBlockHillockFortification.Text = BetrayalResources.HillockFortification;
            textBlockHillockFortification.ToolTip = BetrayalResources.HillockFortificationTooltip;
            textBlockHillockIntervention.Text = BetrayalResources.HillockIntervention;
            textBlockHillockIntervention.ToolTip = BetrayalResources.HillockInterventionTooltip;
            textBlockHillockResearch.Text = BetrayalResources.HillockResearch;
            textBlockHillockResearch.ToolTip = BetrayalResources.HillockResearchTooltip;
            textBlockHillockTransportation.Text = BetrayalResources.HillockTransportation;
            textBlockHillockTransportation.ToolTip = BetrayalResources.HillockTransportationTooltip;

            textBlockItThatFledFortification.Text = BetrayalResources.ItThatFledFortification;
            textBlockItThatFledIntervention.Text = BetrayalResources.ItThatFledIntervention;
            textBlockItThatFledResearch.Text = BetrayalResources.ItThatFledResearch;
            textBlockItThatFledResearch.ToolTip = BetrayalResources.ItThatFledResearchTooltip;
            textBlockItThatFledTransportation.Text = BetrayalResources.ItThatFledTransportation;

            textBlockJanusFortification.Text = BetrayalResources.JanusFortification;
            textBlockJanusIntervention.Text = BetrayalResources.JanusIntervention;
            textBlockJanusResearch.Text = BetrayalResources.JanusResearch;
            textBlockJanusTransportation.Text = BetrayalResources.JanusTransportaion;

            textBlockJorginFortification.Text = BetrayalResources.JorginFortification;
            textBlockJorginIntervention.Text = BetrayalResources.JorginIntervention;
            textBlockJorginResearch.Text = BetrayalResources.JorginResearch;
            textBlockJorginResearch.ToolTip = BetrayalResources.JorginResearchTooltip;
            textBlockJorginTransportation.Text = BetrayalResources.JorginTransportation;

            textBlockKorrelFortifcation.Text = BetrayalResources.KorrelFortification;
            textBlockKorrelIntervention.Text = BetrayalResources.KorrelIntervention;
            textBlockKorrelResearch.Text = BetrayalResources.KorrellResearch;
            textBlockKorrelTransportation.Text = BetrayalResources.KorrellTransportation;

            textBlockLeoFortification.Text = BetrayalResources.LeoFortification;
            textBlockLeoIntervention.Text = BetrayalResources.LeoIntervention;
            textBlockLeoResearch.Text = BetrayalResources.LeoResearch;
            textBlockLeoResearch.ToolTip = BetrayalResources.LeoResearchTooltip;
            textBlockLeoTransportation.Text = BetrayalResources.LeoTransportation;

            textBlockRikerFortification.Text = BetrayalResources.RikerFortification;
            textBlockRikerIntervention.Text = BetrayalResources.RikerIntervention;
            textBlockRikerResearch.Text = BetrayalResources.RikerResearch;
            textBlockRikerTransportation.Text = BetrayalResources.RikerTransportation;

            textBlockRinFortification.Text = BetrayalResources.RinFortification;
            textBlockRinIntervention.Text = BetrayalResources.RinIntervention;
            textBlockRinResearch.Text = BetrayalResources.RinResearch;
            textBlockRinTransportation.Text = BetrayalResources.RinTransportation;

            textBlockToraFortification.Text = BetrayalResources.ToraFortification;
            textBlockToraFortification.ToolTip = BetrayalResources.ToraFortificationTooltip;
            textBlockToraIntervention.Text = BetrayalResources.ToraIntervention;
            textBlockToraResearch.Text = BetrayalResources.ToraResearch;
            textBlockToraResearch.ToolTip = BetrayalResources.ToraResearchTooltip;
            textBlockToraTransportation.Text = BetrayalResources.ToraTransportation;
            textBlockToraTransportation.ToolTip = BetrayalResources.ToraTransportationTooltip;

            textBlockVaganFortification.Text = BetrayalResources.VaganFortification;
            textBlockVaganIntervention.Text = BetrayalResources.VaganIntervention;
            textBlockVaganResearch.Text = BetrayalResources.VaganResearch;
            textBlockVaganTransportation.Text = BetrayalResources.VaganTransportation;

            textBlockVoriciFortification.Text = BetrayalResources.VoriciFortification;
            textBlockVoriciIntervention.Text = BetrayalResources.VoriciIntervention;
            textBlockVoriciResearch.Text = BetrayalResources.VoriciResearch;
            textBlockVoriciResearch.ToolTip = BetrayalResources.VoriceResearchTooltip;
            textBlockVoriciTransportation.Text = BetrayalResources.VoriciTransportation;
        }

        private void UpdateBlightUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelClearOil.Content = Legacy.UILanguageProvider.Language.BlightClearOil;
            textBlockClearOilEffect.Text = Legacy.UILanguageProvider.Language.BlightClearOilEffect;

            labelSepiaOil.Content = Legacy.UILanguageProvider.Language.BlightSepiaOil;
            textBlockSepiaOilEffect.Text = Legacy.UILanguageProvider.Language.BlightSepiaOilEffect;

            labelAmberOil.Content = Legacy.UILanguageProvider.Language.BlightAmberOil;
            textBlockAmberOilEffect.Text = Legacy.UILanguageProvider.Language.BlightAmberOilEffect;

            labelVerdantOil.Content = Legacy.UILanguageProvider.Language.BlightVerdantOil;
            textBlockVerdantOilEffect.Text = Legacy.UILanguageProvider.Language.BlightVerdantOilEffect;

            labelTealOil.Content = Legacy.UILanguageProvider.Language.BlightTealOil;
            textBlockTealOilEffect.Text = Legacy.UILanguageProvider.Language.BlightTealOilEffect;

            labelAzureOil.Content = Legacy.UILanguageProvider.Language.BlightAzureOil;
            textBlockAzureOilEffect.Text = Legacy.UILanguageProvider.Language.BlightAzureOilEffect;

            labelVioletOil.Content = Legacy.UILanguageProvider.Language.BlightVioletOil;
            textBlockVioletOilEffect.Text = Legacy.UILanguageProvider.Language.BlightVioletOilEffect;

            labelCrimsonOil.Content = Legacy.UILanguageProvider.Language.BlightCrimsonOil;
            textBlockCrimsonOilEffect.Text = Legacy.UILanguageProvider.Language.BlightCrimsonOilEffect;

            labelBlackOil.Content = Legacy.UILanguageProvider.Language.BlightBlackOil;
            textBlockBlackOilEffect.Text = Legacy.UILanguageProvider.Language.BlightBlackOilEffect;

            labelOpalescentOil.Content = Legacy.UILanguageProvider.Language.BlightOpalescentOil;
            textBlockOpalescentOilEffect.Text = Legacy.UILanguageProvider.Language.BlightOpalescentOilEffect;

            labelSilverOil.Content = Legacy.UILanguageProvider.Language.BlightSilverOil;
            textBlockSilverOilEffect.Text = Legacy.UILanguageProvider.Language.BlightSilverOilEffect;

            labelGoldenOil.Content = Legacy.UILanguageProvider.Language.BlightGoldenOil;
            textBlockGoldenOilEffect.Text = Legacy.UILanguageProvider.Language.BlightGoldenOilEffect;
        }

        private void UpdateMetamorphUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            labelAbrasiveCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphAbrasiveCatalyst;
            textBlockAbrasiveCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphAbrasiveCatalystEffect;

            labelFertileCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphFertileCatalyst;
            textBlockFertileCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphFertileCatalyst;

            labelImbuedCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphImbuedCatalyst;
            textBlockImbuedCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphImbuedCatalystEffect;

            labelIntrinsicCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphIntrinsicCatalyst;
            textBlockIntrinsicCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphIntrinsicCatalystEffect;

            labelPrismaticCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphPrismaticCatalyst;
            textBlockPrismaticCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphPrismaticCatalystEffect;

            labelTemperingCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphTemperingCatalyst;
            textBlockTemperingCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphTemperingCatalystEffect;

            labelTurbulentCatalyst.Content = Legacy.UILanguageProvider.Language.MetamorphTurbulentCatalyst;
            textBlockTurbulentCatalystEffect.Text = Legacy.UILanguageProvider.Language.MetamorphTurbulentCatalystEffect;

            labelMetamorphInformation.Content = Legacy.UILanguageProvider.Language.MetamorphInformationHeader;
            textBlockMetamorphInformationText.Text = Legacy.UILanguageProvider.Language.MetamorphInformationText;
        }

        private void UpdateDelveUIText()
        {
            var cultureInfo = new CultureInfo(Legacy.UILanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            string fracturedWallInfoPointer = "*";

            var mineFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveMetallicFossil,
                Legacy.UILanguageProvider.Language.DelveSerratedFossil,
                Legacy.UILanguageProvider.Language.DelvePristineFossil,
                Legacy.UILanguageProvider.Language.DelveAethericFossil,
            };

            var magmaFissureFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveScorchedFossil,
                Legacy.UILanguageProvider.Language.DelvePristineFossil,
                Legacy.UILanguageProvider.Language.DelvePrismaticFossil,
                Legacy.UILanguageProvider.Language.DelveEnchantedFossil,
                Legacy.UILanguageProvider.Language.DelveEncrustedFossil + fracturedWallInfoPointer,
            };

            var sulfurVentsFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveMetallicFossil,
                Legacy.UILanguageProvider.Language.DelveAethericFossil,
                Legacy.UILanguageProvider.Language.DelvePerfectFossil,
                Legacy.UILanguageProvider.Language.DelveEncrustedFossil + fracturedWallInfoPointer,
            };

            var frozenHollowFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveFrigidFossil,
                Legacy.UILanguageProvider.Language.DelveSerratedFossil,
                Legacy.UILanguageProvider.Language.DelvePrismaticFossil,
                Legacy.UILanguageProvider.Language.DelveShudderingFossil,
                Legacy.UILanguageProvider.Language.DelveSanctifiedFossil + fracturedWallInfoPointer,
            };

            var fungalCavernsFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveDenseFossil,
                Legacy.UILanguageProvider.Language.DelveAberrantFossil,
                Legacy.UILanguageProvider.Language.DelvePerfectFossil,
                Legacy.UILanguageProvider.Language.DelveCorrodedFossil,
                Legacy.UILanguageProvider.Language.DelveGildedFossil + fracturedWallInfoPointer,
            };

            var petrifiedForestFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveBoundFossil,
                Legacy.UILanguageProvider.Language.DelveDenseFossil,
                Legacy.UILanguageProvider.Language.DelveJaggedFossil,
                Legacy.UILanguageProvider.Language.DelveCorrodedFossil,
                Legacy.UILanguageProvider.Language.DelveSanctifiedFossil + fracturedWallInfoPointer,
            };

            var abyssalDepthsFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveBoundFossil,
                Legacy.UILanguageProvider.Language.DelveAberrantFossil,
                Legacy.UILanguageProvider.Language.DelveLucentFossil + fracturedWallInfoPointer,
                Legacy.UILanguageProvider.Language.DelveGildedFossil + fracturedWallInfoPointer,
            };

            var fossilRoomFossils = new[]
            {
                Legacy.UILanguageProvider.Language.DelveGlyphicFossil,
                Legacy.UILanguageProvider.Language.DelveFracturedFossil,
                Legacy.UILanguageProvider.Language.DelveFacetedFossil,
                Legacy.UILanguageProvider.Language.DelveBloodstainedFossil,
                Legacy.UILanguageProvider.Language.DelveTangledFossil,
                Legacy.UILanguageProvider.Language.DelveHollowFossil,
            };

            labelDelveMines.Content = Legacy.UILanguageProvider.Language.DelveMines;
            SetTextBlockList(textBlockDelveMinesFossils, mineFossils);

            labelDelveMagmaFissure.Content = Legacy.UILanguageProvider.Language.DelveMagmaFissure;
            SetTextBlockList(textBlockMagmaFissureFossils, magmaFissureFossils);

            labelDelveSulfurVents.Content = Legacy.UILanguageProvider.Language.DelveSulfurVents;
            SetTextBlockList(textBlockSulfurVentsFossils, sulfurVentsFossils);

            labelDelveFrozenHollow.Content = Legacy.UILanguageProvider.Language.DelveFrozenHollow;
            SetTextBlockList(textBlockFrozenHolloeFossils, frozenHollowFossils);

            labelDelveFungalCaverns.Content = Legacy.UILanguageProvider.Language.DelveFungalCaverns;
            SetTextBlockList(textBlockDelveFungalCavernsFossils, fungalCavernsFossils);

            labelDelvePetrifiedForest.Content = Legacy.UILanguageProvider.Language.DelvePetrifiedForest;
            SetTextBlockList(labelDelvePetrifiedForestFossils, petrifiedForestFossils);

            labelDelveAbyssalDepths.Content = Legacy.UILanguageProvider.Language.DelveAbyssalDepths;
            SetTextBlockList(textBlockAbyssalDepthsFossils, abyssalDepthsFossils);

            labelDelveFossilRoom.Content = Legacy.UILanguageProvider.Language.DelveFossilRoom;
            SetTextBlockList(textBlockDelveFossilRoomFossils, fossilRoomFossils);

            labelDelveInformation.Content = Legacy.UILanguageProvider.Language.DelveInformation;
        }
    }
}
