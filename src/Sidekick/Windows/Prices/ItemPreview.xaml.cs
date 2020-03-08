using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.UI.Prices;

namespace Sidekick.Windows.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemPreview : UserControl
    {
        public IPriceViewModel ViewModel { get; set; }

        public ItemPreview()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
