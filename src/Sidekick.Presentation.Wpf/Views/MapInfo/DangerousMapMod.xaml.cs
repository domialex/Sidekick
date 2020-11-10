using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Views.MapInfo
{
    /// <summary>
    /// Interaction logic for DangerousMapMod.xaml
    /// </summary>
    [DependencyProperty]
    public partial class DangerousMapMod : UserControl
    {
        public DangerousMapMod()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public DangerousMapModModel Model { get; set; }
    }
}
