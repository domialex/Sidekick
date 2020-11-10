using System;
using System.Windows;
using MediatR;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Wpf.Views;

namespace Sidekick.Presentation.Wpf.Initialization
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class InitializationView : BaseOverlay
    {
        private readonly IViewLocator viewLocator;
        private readonly IMediator mediator;

        public InitializationView(
            InitializationViewModel viewModel,
            IViewLocator viewLocator,
            IMediator mediator,
            IServiceProvider serviceProvider)
            : base("Initialization", serviceProvider)
        {
            this.viewLocator = viewLocator;
            this.mediator = mediator;
            InitializeComponent();
            DataContext = viewModel;
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
