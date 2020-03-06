using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Localization;

namespace Sidekick.UI.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private bool isDisposed;

        public ViewLocator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            Views = new List<ViewInstance>();
        }

        private List<ViewInstance> Views { get; set; }

        public void Open<TView>()
            where TView : ISidekickView
        {
            // Still needed for localization of league overlay models
            Thread.CurrentThread.CurrentCulture = TranslationSource.Instance.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = TranslationSource.Instance.CurrentCulture;

            var view = new ViewInstance(
                serviceProvider.CreateScope(),
                typeof(TView)
            );

            view.Disposed += () =>
            {
                Views.Remove(view);
            };

            Views.Add(view);
        }

        public bool IsOpened<TView>()
        {
            return Views.Any(x => x.ViewType == typeof(TView));
        }

        public void CloseAll()
        {
            for (var i = Views.Count; i > 0; i--)
            {
                Views[i - 1].View.Close();
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
