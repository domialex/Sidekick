using System;
using System.Diagnostics;
using System.Reflection;
using Bindables;
using MediatR;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Views.About
{
    [DependencyProperty]
    public partial class AboutView : BaseView, ISidekickView
    {
        private readonly IMediator mediator;

        public AboutView(
            IServiceProvider serviceProvider,
            IMediator mediator)
            : base("about", serviceProvider)
        {
            DataContext = this;

            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            VersionNumber = fileVersionInfo.ProductVersion;

            try
            {
                OperatingSystem = Environment.OSVersion.VersionString;
                EnvironmentVersion = Environment.Version.ToString();
            }
            catch (Exception)
            {
                // Getting the operating system can fail
            }

            this.mediator = mediator;
        }

        public string VersionNumber { get; private set; }
        public string OperatingSystem { get; private set; }
        public string EnvironmentVersion { get; private set; }

        private async void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            await mediator.Send(new OpenBrowserCommand(e.Uri));
        }
    }
}