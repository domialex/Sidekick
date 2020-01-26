using System;
using System.Windows;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty PropertyProperty =
      DependencyProperty.Register(nameof(Property), typeof(string), typeof(KeybindEditor), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value), typeof(string), typeof(KeybindEditor), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public KeybindEditor()
        {
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
            if (key.EndsWith("+"))
            {
                Value = PreviousValue;
                return;
            }

            Value = key;
            HotkeyChanged?.Invoke(this);
        }

        /// <summary>
        /// Starts to capture keyboard input when the button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PreviousValue = Value;
            Value = null;
            HotkeyChanging?.Invoke(this);
        }
    }
}
