using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Delve;

namespace Sidekick.Views.Leagues.Delve
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

        public DelveLeague Model { get; set; }
    }
}
