using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Incursion;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Incursion
{
    /// <summary>
    /// Interaction logic for Room.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Room : UserControl
    {
        public Room()
        {
            InitializeComponent();
            Container.DataContext = this;
            Loaded += Room_Loaded;
        }

        public IncursionRoom Model { get; set; }

        private void Room_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Model.ContainsTooltip))
            {
                Container.ToolTip = Model.ContainsTooltip;
            }
        }
    }
}
