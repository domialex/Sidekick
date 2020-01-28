using System.Windows;
using Sidekick.UI.Settings;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(KeybindEditor), new PropertyMetadata(""));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(string), typeof(KeybindEditor), new PropertyMetadata(""));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

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
