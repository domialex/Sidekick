using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Delve;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Delve
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
