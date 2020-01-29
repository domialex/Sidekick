using System.Windows;
using Bindables;
using Sidekick.UI.Settings;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    [DependencyProperty]
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public KeybindEditor()
        {
            InitializeComponent();
            Grid.DataContext = this;
        }

        public ISettingsViewModel ViewModel { get; set; }

        public string Key { get; set; }

        public void Capture(string key)
        {
            // If no actual key was pressed - return
            if (key == "Esc" || key.EndsWith("+"))
            {
                // Value = PreviousValue;
                return;
            }

            // Value = key;
        }

        private void HotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            // PreviousValue = Value;
            // Value = null;
        }
    }
}
