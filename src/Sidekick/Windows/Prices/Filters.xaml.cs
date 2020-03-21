using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Prices;

namespace Sidekick.Windows.Prices
{
    /// <summary>
    /// Interaction logic for Filters.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Filters : UserControl
    {
        public IPriceViewModel ViewModel { get; set; }

        public Filters()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
