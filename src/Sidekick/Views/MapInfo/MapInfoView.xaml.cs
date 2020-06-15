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
    public partial class MapInfoView : BaseOverlay
    {
        public MapInfoView(MapInfoViewModel mapInfoViewModel, IServiceProvider serviceProvider)
            : base("map_info", serviceProvider)
        {
            InitializeComponent();
            ViewModel = mapInfoViewModel;
            DataContext = ViewModel;

            Show();
            Activate();

            if (GetMouseXPercent() > 0.5)
            {
                SetLeftPercent(66, LocationSource.End);
            }
            else
            {
                SetLeftPercent(34, LocationSource.Begin);
            }
            SetTopPercent(50, LocationSource.Center);
            

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
