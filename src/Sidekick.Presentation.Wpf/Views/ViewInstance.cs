using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Wpf.About;
using Sidekick.Presentation.Wpf.Cheatsheets;
using Sidekick.Presentation.Wpf.Initialization;
using Sidekick.Presentation.Wpf.Settings;
using Sidekick.Presentation.Wpf.Setup;
using Sidekick.Presentation.Wpf.Views.ApplicationLogs;
using Sidekick.Presentation.Wpf.Views.MapInfo;
using Sidekick.Presentation.Wpf.Views.Prices;

namespace Sidekick.Presentation.Wpf.Views
{
    public class ViewInstance : IDisposable
    {
        private static readonly Dictionary<View, Type> ViewTypes = new Dictionary<View, Type>() {
            { Domain.Views.View.About, typeof(AboutView) },
            { Domain.Views.View.Initialization, typeof(InitializationView) },
            { Domain.Views.View.Map, typeof(MapInfoView) },
            { Domain.Views.View.League, typeof(LeagueView) },
            { Domain.Views.View.Logs, typeof(ApplicationLogsView) },
            { Domain.Views.View.ParserError, typeof(Errors.ParserError) },
            { Domain.Views.View.Price, typeof(PriceView) },
            { Domain.Views.View.Settings, typeof(SettingsView) },
            { Domain.Views.View.Setup, typeof(SetupView) },
        };

        private readonly ViewLocator viewLocator;

        public ViewInstance(ViewLocator viewLocator, IServiceProvider serviceProvider, View view, params object[] args)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ViewInstance>>();
            var settings = serviceProvider.GetRequiredService<ISidekickSettings>();

            if (!ViewTypes.ContainsKey(view))
            {
                logger.LogError($"The view {view} could not be opened.");
                return;
            }

            this.viewLocator = viewLocator;
            Scope = serviceProvider.CreateScope();

            // Still needed for localization of league overlay models
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.Language_UI);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.Language_UI);

            // View initialization and show
            View = (ISidekickView)Scope.ServiceProvider.GetRequiredService(ViewTypes[view]);
            View.Closed += View_Closed;

            Task.Run(async () =>
            {
                try
                {
                    await View.Open(args);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"The view {view} could not be opened. {e.Message}");
                    Dispose();
                }
            });
        }

        private IServiceScope Scope { get; set; }

        public ISidekickView View { get; set; }

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
            if (View != null)
            {
                View.Closed -= View_Closed;
                View.Close();
            }
            viewLocator.Views.Remove(this);
            Scope?.Dispose();
        }
    }
}
