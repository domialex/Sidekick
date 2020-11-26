using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    /// <summary>
    /// Interaction logic for Agent.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ItemPreview : UserControl
    {
        public PriceViewModel ViewModel { get; set; }

        public ItemPreview()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
