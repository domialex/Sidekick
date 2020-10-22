using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Settings;

namespace Sidekick.Views
{
    public class ViewInstance<TView> : IViewInstance
        where TView : ISidekickView
    {
        private readonly ViewLocator viewLocator;

        public ViewInstance(ViewLocator viewLocator, IServiceProvider serviceProvider)
        {
            this.viewLocator = viewLocator;
            Scope = serviceProvider.CreateScope();

            // Still needed for localization of league overlay models
            var settings = Scope.ServiceProvider.GetService<SidekickSettings>();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.Language_UI);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.Language_UI);

            // View initialization and show
            View = Scope.ServiceProvider.GetService<TView>();
            View.Closed += View_Closed;
            View.Show();
        }

        private IServiceScope Scope { get; set; }

        private ISidekickView View { get; set; }

        public Type Type => typeof(TView);

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
            }
            View.Close();
            viewLocator.Views.Remove(this);
            Scope?.Dispose();
        }
    }
}
