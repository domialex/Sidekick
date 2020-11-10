using System.Threading.Tasks;
using System.Windows;
using MediatR;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Views;
using Sidekick.Views;

namespace Sidekick.Initialization
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class InitializationView : Window, ISidekickView
    {
        private readonly IViewLocator viewLocator;
        private readonly IMediator mediator;

        public InitializationView(
            InitializationViewModel viewModel,
            IViewLocator viewLocator,
            IMediator mediator)
        {
            this.viewLocator = viewLocator;
            this.mediator = mediator;
            InitializeComponent();
            DataContext = viewModel;
        }

        public Task Open(params object[] args)
        {
            Show();
            return Task.CompletedTask;
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            await mediator.Send(new ShutdownCommand());
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            viewLocator.Open(View.Logs);
        }
    }
}
