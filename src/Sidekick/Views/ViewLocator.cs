using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Settings;

namespace Sidekick.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SidekickSettings settings;
        private bool isDisposed;

        public ViewLocator(IServiceProvider serviceProvider,
            SidekickSettings settings)
        {
            this.serviceProvider = serviceProvider;
            this.settings = settings;
            Views = new List<ViewInstance>();
        }

        private List<ViewInstance> Views { get; set; }

        public void Open<TView>()
            where TView : ISidekickView
        {
            // Still needed for localization of league overlay models
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.Language_UI);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.Language_UI);

            var view = new ViewInstance(
                serviceProvider.CreateScope(),
                typeof(TView)
            );

            view.Disposed += () =>
            {
                Views.Remove(view);
            };

            Views.Add(view);

            view.Open();
        }

        public bool IsOpened<TView>()
        {
            return Views.Any(x => x.ViewType == typeof(TView));
        }

        public void CloseAll()
        {
            while (Views.Count > 0)
            {
                var view = Views[0];
                view.View.Close();
                view.Dispose();
                if (Views.Contains(view))
                {
                    Views.Remove(view);
                }
            }
        }

        public void Close<TView>()
            where TView : ISidekickView
        {
            foreach (var view in Views.Where(x => x.ViewType == typeof(TView)))
            {
                view.View.Close();
                view.Dispose();
                if (Views.Contains(view))
                {
                    Views.Remove(view);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (Views != null)
                {
                    foreach (var view in Views)
                    {
                        view.Dispose();
                    }
                }
            }

            isDisposed = true;
        }
    }
}
