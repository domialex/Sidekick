using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Delve;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Delve
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
