using System.Windows;
using System.Windows.Media;
using Bindables;
using Sidekick.Localization.Settings;
using Sidekick.UI.Settings;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    [DependencyProperty]
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public ISettingsViewModel ViewModel { get; set; }

        public string Label
        {
            get
            {
                var resourceValue = SettingResources.ResourceManager.GetString(Key);
                if (string.IsNullOrEmpty(resourceValue))
                {
                    return Key;
                }
                return resourceValue;
            }
        }

        public SolidColorBrush BackgroundColor { get; private set; }

        public KeybindEditor()
        {
            InitializeComponent();
            Grid.DataContext = this;
            Unloaded += KeybindEditor_Unloaded;
            Loaded += KeybindEditor_Loaded;
        }

        private void KeybindEditor_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentKey))
            {
                if (ViewModel != null && Key == ViewModel.CurrentKey)
                {
                    BackgroundColor = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                }
                else
                {
                    BackgroundColor = new SolidColorBrush(Color.FromArgb(0, 200, 200, 200));
                }
            }
        }

        private void KeybindEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentKey = null;
        }

        private void HotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentKey = Key;
        }
    }
}
