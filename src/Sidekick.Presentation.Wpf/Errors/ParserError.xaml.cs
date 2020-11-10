using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Sidekick.Views;

namespace Sidekick.Errors
{
    public partial class ParserError : BaseOverlay
    {
        public ParserError(
            IServiceProvider serviceProvider,
            Dispatcher dispatcher)
            : base("parserError", serviceProvider)
        {
            InitializeComponent();

            if (GetMouseXPercent() > 0.5)
            {
                SetLeftPercent(0.654, LocationSource.End);
            }
            else
            {
                SetLeftPercent(0.346, LocationSource.Begin);
            }
            SetTopPercent(50, LocationSource.Center);

            dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(1500);
                Close();
            });
        }

        public override Task Open(params object[] args)
        {
            Show();
            Activate();

            if (GetMouseXPercent() > 0.5)
            {
                SetLeftPercent(0.654, LocationSource.End);
            }
            else
            {
                SetLeftPercent(0.346, LocationSource.Begin);
            }
            SetTopPercent(50, LocationSource.Center);

            return Task.CompletedTask;
        }
    }
}
