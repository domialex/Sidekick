using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Metamorph;

namespace Sidekick.Views.Leagues.Metamorph
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

        public MetamorphLeague Model { get; set; }
    }
}
