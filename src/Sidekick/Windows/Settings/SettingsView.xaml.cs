using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Sidekick.Business.Languages.UI;
using Sidekick.UI.Settings;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Settings
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SettingsView : Window, ISidekickView
    {
        private const int WINDOW_WIDTH = 480;
        private const int WINDOW_HEIGHT = 320;
        private readonly ISettingsViewModel viewModel;
        private readonly IUILanguageProvider uiLanguageProvider;

        public SettingsView(ISettingsViewModel viewModel,
            IUILanguageProvider uiLanguageProvider)
        {
            this.viewModel = viewModel;
            this.uiLanguageProvider = uiLanguageProvider;

            Width = WINDOW_WIDTH;
            Height = WINDOW_HEIGHT;

            InitializeComponent();
            DataContext = viewModel;

            SetUILanguage();

            ElementHost.EnableModelessKeyboardInterop(this);

            Show();
        }

        private void SetUILanguage()
        {
            tabItemGeneral.Header = uiLanguageProvider.Language.SettingsWindowTabGeneral;
            tabItemKeybindings.Header = uiLanguageProvider.Language.SettingsWindowTabKeybindings;
            // groupBoxWikiSettings.Header = uiLanguageProvider.Language.SettingsWindowWikiSettings;
            // groupBoxLanguageSettings.Header = uiLanguageProvider.Language.SettingsWindowLanguageSettings;
            // labelWikiDescription.Content = uiLanguageProvider.Language.SettingsWindowWikiDescription;
            // labelLanguageDescription.Content = uiLanguageProvider.Language.SettingsWindowLanguageDescription;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
            Close();
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
