using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Views.Leagues.Delve
{
    /// <summary>
    /// Interaction logic for Region.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Region : UserControl
    {
        public Region()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public DelveRegion Model { get; set; }
    }
}
