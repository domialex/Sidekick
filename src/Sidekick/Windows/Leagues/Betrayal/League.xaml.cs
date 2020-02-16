using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Betrayal;

namespace Sidekick.Windows.Leagues.Betrayal
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

        public BetrayalLeague Model { get; set; }
    }
}
