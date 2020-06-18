using System;
using System.Diagnostics;
using System.Reflection;
using Bindables;

namespace Sidekick.Views.About
{
    [DependencyProperty]
    public partial class AboutView : BaseWindow, ISidekickView
    {
        public AboutView(IServiceProvider serviceProvider)
            : base("about", serviceProvider)
        {
            DataContext = this;

            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
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

            Show();
        }

        public string VersionNumber { get; private set; }
        public string OperatingSystem { get; private set; }
        public string EnvironmentVersion { get; private set; }
    }
}
