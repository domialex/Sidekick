using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Blight
{
    /// <summary>
    /// Interaction logic for League.xaml
    /// </summary>
    [DependencyProperty]
    public partial class League : UserControl
    {
        public League()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public BlightLeague Model { get; set; }
    }
}
