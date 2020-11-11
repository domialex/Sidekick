using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Views.Prices
{
    /// <summary>
    /// Interaction logic for Filters.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Filters : UserControl
    {
        public PriceViewModel ViewModel { get; set; }

        public Filters()
        {
            InitializeComponent();
            Container.DataContext = this;
        }
    }
}
