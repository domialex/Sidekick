using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Settings;
using Sidekick.Platforms;
using Sidekick.UI.Settings;
using Sidekick.UI.Views;
using Sidekick.Windows.Settings.UserControls;

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
            IUILanguageProvider uiLanguageProvider,
            INativeKeyboard nativeKeyboard)
        {
            this.viewModel = viewModel;
            this.uiLanguageProvider = uiLanguageProvider;

            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;

            Width = WINDOW_WIDTH;
            Height = WINDOW_HEIGHT;

            InitializeComponent();
            DataContext = viewModel;

            SetUILanguage();
            SelectWikiSetting();
            InitializeUILanguageCombobox();

            ElementHost.EnableModelessKeyboardInterop(this);

            Show();
        }

        private Task NativeKeyboard_OnKeyDown(string key)
        {
            if (CurrentKeybind != null)
            {
                CurrentKeybind.Capture(key);
            }
            return Task.CompletedTask;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SetUILanguage()
        {
            tabItemGeneral.Header = uiLanguageProvider.Language.SettingsWindowTabGeneral;
            tabItemKeybindings.Header = uiLanguageProvider.Language.SettingsWindowTabKeybindings;
            groupBoxWikiSettings.Header = uiLanguageProvider.Language.SettingsWindowWikiSettings;
            groupBoxLanguageSettings.Header = uiLanguageProvider.Language.SettingsWindowLanguageSettings;
            labelWikiDescription.Content = uiLanguageProvider.Language.SettingsWindowWikiDescription;
            labelLanguageDescription.Content = uiLanguageProvider.Language.SettingsWindowLanguageDescription;
        }

        private void SelectWikiSetting()
        {
            if (viewModel.Settings.CurrentWikiSettings == WikiSetting.PoeWiki)
            {
                radioButtonPOEWiki.IsChecked = true;
            }
            else if (viewModel.Settings.CurrentWikiSettings == WikiSetting.PoeDb)
            {
                radioButtonPOEDb.IsChecked = true;
            }
        }

        private void InitializeUILanguageCombobox()
        {
            comboBoxUILanguages.ItemsSource = uiLanguageProvider.AvailableLanguages.Select(x => x.Name);
            comboBoxUILanguages.SelectedItem = viewModel.Settings.UILanguage;
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

        private KeybindEditor CurrentKeybind { get; set; }

        private void KeybindEditor_HotkeyChanging(KeybindEditor keybind)
        {
            CurrentKeybind = keybind;
        }

        /// <summary>
        /// Gets called after a new hotkey has been defined. Checks if that new hotkey is unique
        /// </summary>
        /// <param name="sender"></param>
        private void KeybindEditor_HotkeyChanged(KeybindEditor keybind)
        {
            if (viewModel.IsKeybindUsed(keybind.Value, keybind.Property))
            {
                if (MessageBox.Show("This hotkey already in use.") == MessageBoxResult.OK)
                {
                    keybind.Value = keybind.PreviousValue;
                }
            }
            CurrentKeybind = null;
        }
    }
}
