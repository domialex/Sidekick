using System;
using System.Diagnostics;
using System.Reflection;
using Bindables;
using Sidekick.Core.Natives;

namespace Sidekick.Views.About
{
    [DependencyProperty]
    public partial class AboutView : BaseView, ISidekickView
    {
        private readonly INativeBrowser browser;

        public AboutView(IServiceProvider serviceProvider, INativeBrowser browser)
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

            this.browser = browser;
        }

        public string VersionNumber { get; private set; }
        public string OperatingSystem { get; private set; }
        public string EnvironmentVersion { get; private set; }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            browser.Open(e.Uri);
        }
    }
}
