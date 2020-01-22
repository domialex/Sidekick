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
        public LeagueOverlayView()
        {
            InitializeComponent();
            UpdateBetrayalUIText();
            SettingsController.GetSettingsInstance().CurrentUILanguageProvider.UILanguageChanged += UpdateBetrayalUIText;
        }

        private Task UpdateBetrayalUIText()
        {
            var settings = SettingsController.GetSettingsInstance();

            textBlockAislingTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingTransportaion;
            textBlockAislingFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingFortification;
            textBlockAislingResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingResearch;
            textBlockAislingIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalAislingIntervention;

            textBlockCameriaFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaFortification;
            textBlockCameriaIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaIntervention;
            textBlockCameriaResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaResearch;
            textBlockCameriaTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalCameriaTransportation;

            textBlockElreonFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonFortification;
            textBlockElreonIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonIntervention;
            textBlockElreonResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonResearch;
            textBlockElreonTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalElreonTransportation;

            textBlockGraviciusFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusFortification;
            textBlockGraviciusIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusIntervention;
            textBlockGraviciusResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusResearch;
            textBlockGraviciusTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalGraviciusTransportation;

            textBlockGuffFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffFortification;
            textBlockGuffIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffIntervention;
            textBlockGuffResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffResearch;
            textBlockGuffTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalGuffTransportation;

            textBlockHakuFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuFortification;
            textBlockHakuIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuIntervention;
            textBlockHakuResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuResearch;
            textBlockHakuTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalHakuTransportation;

            textBlockHillockFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockFortification;
            textBlockHillockIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockIntervention;
            textBlockHillockResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockResearch;
            textBlockHillockTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalHillockTransportation;

            textBlockItThatFledFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledFortification;
            textBlockItThatFledIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledIntervention;
            textBlockItThatFledResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledResearch;
            textBlockItThatFledTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalItThatFledTransportation;

            textBlockJanusFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusFortification;
            textBlockJanusIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusIntervention;
            textBlockJanusResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusResearch;
            textBlockJanusTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalJanusTransportaion;

            textBlockJorginFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginFortification;
            textBlockJorginIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginIntervention;
            textBlockJorginResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginResearch;
            textBlockJorginTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalJorginTransportation;

            textBlockKorrelFortifcation.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrelFortification;
            textBlockKorrelIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrelIntervention;
            textBlockKorrelResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrellResearch;
            textBlockKorrelTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalKorrellTransportation;

            textBlockLeoFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoFortification;
            textBlockLeoIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoIntervention;
            textBlockLeoResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalLeoResearch;
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
            textBlockToraIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraIntervention;
            textBlockToraResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraResearch;
            textBlockToraTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalToraTransportation;

            textBlockVaganFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganFortification;
            textBlockVaganIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganIntervention;
            textBlockVaganResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganResearch;
            textBlockVaganTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalVaganTransportation;

            textBlockVoriciFortification.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciFortification;
            textBlockVoriciIntervention.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciIntervention;
            textBlockVoriciResearch.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciResearch;
            textBlockVoriciTransportation.Text = settings.CurrentUILanguageProvider.Language.BetrayalVoriciTransportation;

            labelLegendHighValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendVeryValuable;
            labelLegendGoodValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendValuable;
            labelLegendNormalValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendLessValuable;
            labelLegendNoValue.Content = settings.CurrentUILanguageProvider.Language.BetrayalLegendNotValuable;

            return Task.CompletedTask;
        }
    }
}
