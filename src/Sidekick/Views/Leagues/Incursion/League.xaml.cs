using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Leagues.Incursion;

namespace Sidekick.Views.Leagues.Incursion
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

        public IncursionLeague Model { get; set; }
    }
}
