using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Betrayal;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Betrayal
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
