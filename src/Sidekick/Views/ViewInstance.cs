using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Settings;
using Sidekick.Initialization;
using Sidekick.Presentation.Views;
using Sidekick.Setup;
using Sidekick.Views.About;
using Sidekick.Views.ApplicationLogs;
using Sidekick.Views.Leagues;
using Sidekick.Views.MapInfo;
using Sidekick.Views.Prices;
using Sidekick.Views.Settings;

namespace Sidekick.Views
{
    public class ViewInstance : IDisposable
    {
        private static readonly Dictionary<View, Type> ViewTypes = new Dictionary<View, Type>() {
            { View.About, typeof(AboutView) },
            { View.Initialization, typeof(InitializationView) },
            { View.Map, typeof(MapInfoView) },
            { View.League, typeof(LeagueView) },
            { View.Logs, typeof(ApplicationLogsView) },
            { View.Price, typeof(PriceView) },
            { View.Settings, typeof(SettingsView) },
            { View.Setup, typeof(SetupView) },
        };

        private readonly ViewLocator viewLocator;

        public ViewInstance(ViewLocator viewLocator, View view, IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<ViewInstance>>();

            if (!ViewTypes.ContainsKey(view))
            {
                logger.LogError($"The view {view} could not be opened.");
                return;
            }

            this.viewLocator = viewLocator;
            View = view;
            Scope = serviceProvider.CreateScope();

            // Still needed for localization of league overlay models
            var settings = Scope.ServiceProvider.GetService<ISidekickSettings>();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.Language_UI);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.Language_UI);

            // View initialization and show
            WpfView = (ISidekickView)Scope.ServiceProvider.GetService(ViewTypes[view]);
            WpfView.Closed += View_Closed;
            WpfView.Show();
        }

        private IServiceScope Scope { get; set; }

        public ISidekickView WpfView { get; set; }

        public View View { get; }

        private void View_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (WpfView != null)
            {
                WpfView.Closed -= View_Closed;
            }
            WpfView.Close();
            viewLocator.Views.Remove(this);
            Scope?.Dispose();
        }
    }
}
