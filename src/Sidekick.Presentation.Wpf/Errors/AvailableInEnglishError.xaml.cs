using System;
using System.Threading.Tasks;
using Sidekick.Presentation.Wpf.Views;

namespace Sidekick.Presentation.Wpf.Errors
{
    public partial class AvailableInEnglishError : BaseOverlay
    {
        public AvailableInEnglishError(IServiceProvider serviceProvider)
            : base(Domain.Views.View.AvailableInEnglishError, serviceProvider)
        {
            InitializeComponent();
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

            await Task.Delay(2000);
            Close();
        }
    }
}
