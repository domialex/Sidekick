using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Presentation.Cheatsheets.Delve;

namespace Sidekick.Presentation.Wpf.Cheatsheets.Delve
{
    /// <summary>
    /// Interaction logic for Fossil.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Fossil : UserControl
    {
        public Fossil()
        {
            InitializeComponent();
        }

        [DependencyProperty(OnPropertyChanged = nameof(OnModelChanged))]
        public DelveFossil Model { get; set; }

        private static void OnModelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var fossil = (Fossil)dependencyObject;
            if (fossil.Model != null && fossil.Model.BehindFracturedWall)
            {
                fossil.BehindFracturedWall.Visibility = Visibility.Visible;
            }
            else
            {
                fossil.BehindFracturedWall.Visibility = Visibility.Hidden;
            }
        }
    }
}
