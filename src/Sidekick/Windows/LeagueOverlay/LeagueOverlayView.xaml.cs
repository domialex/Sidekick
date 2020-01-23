using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sidekick.Windows.Settings;

namespace Sidekick.Windows.LeagueOverlay
{
    /// <summary>
    /// Interaction logic for LeagueOverlayView.xaml
    /// </summary>
    public partial class LeagueOverlayView : Window
    {
        private Dictionary<TabItem, int[]> tabPageSizeDictionary;
        private TabItem CurrentPage;

        public LeagueOverlayView()
        {
            InitializeComponent();
            UpdateBetrayalUIText();
            SettingsController.GetSettingsInstance().CurrentUILanguageProvider.UILanguageChanged += UpdateBetrayalUIText;
            tabPageSizeDictionary = new Dictionary<TabItem, int[]>()
            {
                { tabItemIncursion, new[] { 980, 1030 } },
                { tabItemDelve, new[] { 500, 500 } },
                { tabItemBetrayal, new[] { 520, 1200 } },
                { tabItemBlight, new[] { 500, 500 } },
                { tabItemMetamorph, new[] { 500, 500 } },
            };
            tabControlLeagueOverlay.SelectionChanged += TabControlLeagueOverlay_SelectionChanged;
            CurrentPage = tabItemIncursion;
        }

        private void TabControlLeagueOverlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentPage = tabControlLeagueOverlay.SelectedItem as TabItem;
            SetCurrentPageWindowSize();
        }

        private void SetCurrentPageWindowSize()
        {
            if(CurrentPage != null)
            {
                if(!tabPageSizeDictionary.TryGetValue(CurrentPage, out var windowSize))
                {
                    throw new Exception("Window Size for TabPage is not defined correctly");
                }

                Height = windowSize[0];
                Width = windowSize[1];
                tabControlLeagueOverlay.Height = windowSize[0];
                tabControlLeagueOverlay.Width = windowSize[1];
            }
        }

        private void UpdateBetrayalUIText()
        {
            var settings = SettingsController.GetSettingsInstance();

            textBlockAislingTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingTransportaion;
            textBlockAislingFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingFortification;
            textBlockAislingResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingResearch;
            textBlockAislingResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalAislingResearchTooltip;
            textBlockAislingIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingIntervention;

            textBlockCameriaFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaFortification;
            textBlockCameriaIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaIntervention;
            textBlockCameriaResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaResearch;
            textBlockCameriaTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaTransportation;
            textBlockCameriaTransportation.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalCameriaTransportationTooltip;

            textBlockElreonFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonFortification;
            textBlockElreonIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonIntervention;
            textBlockElreonResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonResearch;
            textBlockElreonTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonTransportation;

            textBlockGraviciusFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusFortification;
            textBlockGraviciusIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusIntervention;
            textBlockGraviciusResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusResearch;
            textBlockGraviciusTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusTransportation;

            textBlockGuffFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffFortification;
            textBlockGuffFortification.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalGuffFortificationTooltip;
            textBlockGuffIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffIntervention;
            textBlockGuffIntervention.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalGuffInterventionTooltip;
            textBlockGuffResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffResearch;
            textBlockGuffResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalGuffResearchTooltip;
            textBlockGuffTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffTransportation;
            textBlockGuffTransportation.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalGuffTransportationTooltip;

            textBlockHakuFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuFortification;
            textBlockHakuIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuIntervention;
            textBlockHakuResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuResearch;
            textBlockHakuTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuTransportation;

            textBlockHillockFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockFortification;
            textBlockHillockFortification.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalHillockFortificationTooltip;
            textBlockHillockIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockIntervention;
            textBlockHillockIntervention.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalHillockInterventionTooltip;
            textBlockHillockResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockResearch;
            textBlockHillockResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalHillockResearchTooltip;
            textBlockHillockTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockTransportation;
            textBlockHillockTransportation.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalHillockTransportationTooltip;

            textBlockItThatFledFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledFortification;
            textBlockItThatFledIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledIntervention;
            textBlockItThatFledResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledResearch;
            textBlockItThatFledResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledResearchTooltip;
            textBlockItThatFledTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledTransportation;

            textBlockJanusFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusFortification;
            textBlockJanusIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusIntervention;
            textBlockJanusResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusResearch;
            textBlockJanusTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusTransportaion;

            textBlockJorginFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginFortification;
            textBlockJorginIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginIntervention;
            textBlockJorginResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginResearch;
            textBlockJorginResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalJorginResearchTooltip;
            textBlockJorginTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginTransportation;

            textBlockKorrelFortifcation.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrelFortification;
            textBlockKorrelIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrelIntervention;
            textBlockKorrelResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrellResearch;
            textBlockKorrelTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrellTransportation;

            textBlockLeoFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoFortification;
            textBlockLeoIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoIntervention;
            textBlockLeoResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoResearch;
            textBlockLeoResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalLeoResearchTooltip;
            textBlockLeoTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoTransportation;

            textBlockRikerFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalRikerFortification;
            textBlockRikerIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalRikerIntervention;
            textBlockRikerResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalRikerResearch;
            textBlockRikerTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalRikerTransportation;

            textBlockRinFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalRinFortification;
            textBlockRinIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalRinIntervention;
            textBlockRinResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalRinResearch;
            textBlockRinTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalRinTransportation;

            textBlockToraFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraFortification;
            textBlockToraFortification.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalToraFortificationTooltip;
            textBlockToraIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraIntervention;
            textBlockToraResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraResearch;
            textBlockToraResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalToraResearchTooltip;
            textBlockToraTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraTransportation;
            textBlockToraTransportation.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalToraTransportationTooltip;

            textBlockVaganFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganFortification;
            textBlockVaganIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganIntervention;
            textBlockVaganResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganResearch;
            textBlockVaganTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganTransportation;

            textBlockVoriciFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciFortification;
            textBlockVoriciIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciIntervention;
            textBlockVoriciResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciResearch;
            textBlockVoriciResearch.ToolTip = settings.CurrentUILanguageProvider.Language.BetrayalVoriceResearchTooltip;
            textBlockVoriciTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciTransportation;

            labelLegendHighValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendVeryValuable;
            labelLegendGoodValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendValuable;
            labelLegendNormalValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendLessValuable;
            labelLegendNoValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendNotValuable;
        }
    }
}
