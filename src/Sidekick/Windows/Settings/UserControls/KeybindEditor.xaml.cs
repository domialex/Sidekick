using System;
using System.Windows;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(string), typeof(KeybindEditor));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(string), typeof(KeybindEditor));

        public KeybindEditor()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event Action<KeybindEditor> HotkeyChanged;
        public event Action<KeybindEditor> HotkeyChanging;

        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string PreviousValue { get; private set; }

        public void Capture(string key)
        {
            // If no actual key was pressed - return
            if (key == "Esc" || key.EndsWith("+"))
            {
                Value = PreviousValue;
                return;
            }

            Value = key;
            HotkeyChanged?.Invoke(this);
        }

        private void HotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousValue = Value;
            Value = null;
            HotkeyChanging?.Invoke(this);
        }
    }
}
