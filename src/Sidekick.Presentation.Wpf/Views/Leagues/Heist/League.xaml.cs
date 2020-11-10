using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Heist
{
    /// <summary>
    /// Interaction logic for Betrayal.xaml
    /// </summary>
    [DependencyProperty]
    public partial class League : UserControl
    {
        public League()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public HeistLeague Model { get; set; }
    }
}
