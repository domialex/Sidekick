using System;
using System.Threading.Tasks;
using System.Windows;
using Bindables;

namespace Sidekick.Views.MapInfo
{
    /// <summary>
    /// Interaction logic for MapInfoView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class MapInfoView : BaseWindow
    {
        public MapInfoView(MapInfoViewModel mapInfoViewModel, IServiceProvider serviceProvider)
            : base(serviceProvider,
                  closeOnBlur: true)
        {
            InitializeComponent();
            ViewModel = mapInfoViewModel;
            DataContext = ViewModel;

            Show();
            Activate();

            if (GetMouseXPercent() > 0.5)
            {
                SetWindowPositionPercent(0.66 - GetWidthPercent(), 0.5 - (GetHeightPercent() / 2));
            }
            else
            {
                SetWindowPositionPercent(0.34, 0.5 - (GetHeightPercent() / 2));
            }
            

            if (ViewModel.IsError)
            {
                Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(1500);
                    Close();
                });
            }
        }

        public MapInfoViewModel ViewModel { get; set; }
    }
}
