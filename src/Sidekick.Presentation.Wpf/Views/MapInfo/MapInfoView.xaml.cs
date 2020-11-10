using System;
using System.Threading.Tasks;
using System.Windows;
using Bindables;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Presentation.Wpf.Views.MapInfo
{
    /// <summary>
    /// Interaction logic for MapInfoView.xaml
    /// </summary>
    [DependencyProperty]
    public partial class MapInfoView : BaseOverlay
    {
        private readonly MapInfoViewModel viewModel;

        public MapInfoView(MapInfoViewModel viewModel, IServiceProvider serviceProvider)
            : base("map_info", serviceProvider)
        {
            this.viewModel = viewModel;
            InitializeComponent();
            DataContext = viewModel;
        }

        public override async Task Open(params object[] args)
        {
            await base.Open(args);

            if (GetMouseXPercent() > 0.5)
            {
                SetLeftPercent(0.654, LocationSource.End);
            }
            else
            {
                SetLeftPercent(0.346, LocationSource.Begin);
            }
            SetTopPercent(50, LocationSource.Center);

            viewModel.Initialize((Item)args[0]);
        }
    }
}
